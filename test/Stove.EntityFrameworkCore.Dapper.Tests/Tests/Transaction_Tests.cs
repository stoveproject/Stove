using System;
using System.Threading.Tasks;

using Shouldly;

using Stove.Dapper.Repositories;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Dapper.Tests.Domain;
using Stove.EntityFrameworkCore.Dapper.Tests.Domain.Events;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;

using Xunit;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Tests
{
	public class Transaction_Tests : StoveEfCoreDapperTestApplicationBase
	{
		private readonly IDapperRepository<Blog> _blogDapperRepository;
		private readonly IRepository<Blog> _blogRepository;
		private readonly IUnitOfWorkManager _uowManager;

		public Transaction_Tests()
		{
			Building(builder => { }).Ok();

			_uowManager = The<IUnitOfWorkManager>();
			_blogRepository = The<IRepository<Blog>>();
			_blogDapperRepository = The<IDapperRepository<Blog>>();
		}

		[Fact]
		public async Task Should_Rollback_Transaction_On_Failure()
		{
			const string exceptionMessage = "This is a test exception!";

			string blogName = Guid.NewGuid().ToString("N");

			try
			{
				using (_uowManager.Begin())
				{
					await _blogRepository.InsertAsync(
						new Blog(blogName, $"http://{blogName}.com/")
					);

					throw new Exception(exceptionMessage);
				}
			}
			catch (Exception ex) when (ex.Message == exceptionMessage)
			{
			}

			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				_blogRepository.FirstOrDefault(x => x.Name == blogName).ShouldBeNull();
				uow.Complete();
			}
		}

		[Fact]
		public void Dapper_and_EfCore_should_work_under_same_unitofwork_and_when_any_exception_appears_then_rollback_should_be_consistent_for_two_orm()
		{
            The<IEventBus>().Register<BlogCreatedEvent>(
                @event =>
                {
                    @event.Name.ShouldBe("Oguzhan_Same_Uow");

                    throw new Exception("Uow Rollback");
                });

            try
			{
				using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
				{
					int blogId = _blogRepository.InsertAndGetId(new Blog("Oguzhan_Same_Uow", "www.oguzhansoykan.com"));

					Blog person = _blogDapperRepository.Get(blogId);

					person.ShouldNotBeNull();

					uow.Complete();
				}
			}
			catch (Exception exception)
			{
				//no handling.
			}

			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				_blogDapperRepository.FirstOrDefault(x => x.Name == "Oguzhan_Same_Uow").ShouldBeNull();
				_blogRepository.FirstOrDefault(x => x.Name == "Oguzhan_Same_Uow").ShouldBeNull();
				uow.Complete();
			}
		}

		[Fact]
		public async Task inline_sql_with_dapper_should_rollback_when_uow_fails()
		{
			//The<IEventBus>().Register<EntityCreatingEvent<Blog>>(
			//	@event => { @event.Entity.Name.ShouldBe("Oguzhan_Same_Uow"); });

			var blogId = 0;

			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				blogId = _blogDapperRepository.InsertAndGetId(new Blog("Oguzhan_Same_Uow", "www.stove.com"));

				Blog person = _blogRepository.Get(blogId);

				person.ShouldNotBeNull();

				uow.Complete();
			}

			try
			{
				using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin(new UnitOfWorkOptions { IsTransactional = true }))
				{
					await _blogDapperRepository.ExecuteAsync("Update Blogs Set Name = @name where Id =@id", new { id = blogId, name = "Oguzhan_New_Blog" });

					throw new Exception("uow rollback");

					uow.Complete();
				}
			}
			catch (Exception exception)
			{
				//no handling.
			}

			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				_blogDapperRepository.FirstOrDefault(x => x.Name == "Oguzhan_New_Blog").ShouldBeNull();
				_blogRepository.FirstOrDefault(x => x.Name == "Oguzhan_New_Blog").ShouldBeNull();

				_blogDapperRepository.FirstOrDefault(x => x.Name == "Oguzhan_Same_Uow").ShouldNotBeNull();
				_blogRepository.FirstOrDefault(x => x.Name == "Oguzhan_Same_Uow").ShouldNotBeNull();

				uow.Complete();
			}
		}
	}
}
