using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Repositories;
using Stove.EntityFrameworkCore.Tests.Domain;
using Stove.EntityFrameworkCore.Tests.Ef;

using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
	public class Repository_The_Tests : EntityFrameworkCoreModuleTestBase
	{
		public Repository_The_Tests()
		{
			Building(builder => { }).Ok();
		}

		[Fact]
		public void Should_The_Custom_Repository_If_Registered()
		{
			var postRepository = The<IPostRepository>();

			postRepository.GetAllList().Any().ShouldBeTrue();

			Assert.Throws<Exception>(() => postRepository.Count()).Message.ShouldBe("can not get count of posts");

			//Should also The by custom interface and implementation
			The<IPostRepository>();
			//The<PostRepository>();
		}

		[Fact]
		public void Should_The_Default_Repositories_For_Second_DbContext()
		{
			var repo1 = The<IRepository<Ticket>>();
			var repo2 = The<IRepository<Ticket, int>>();

			Assert.Throws<Exception>(
				() => repo1.Count()
			).Message.ShouldBe("can not get count!");

			Assert.Throws<Exception>(
				() => repo2.Count()
			).Message.ShouldBe("can not get count!");
		}

		[Fact]
		public void Should_The_Custom_Repositories_For_Second_DbContext()
		{
			var repo1 = The<ISupportRepository<Ticket>>();
			var repo2 = The<ISupportRepository<Ticket, int>>();

			typeof(ISupportRepository<Ticket>).GetTypeInfo().IsInstanceOfType(repo1).ShouldBeTrue();
			typeof(ISupportRepository<Ticket, int>).GetTypeInfo().IsInstanceOfType(repo1).ShouldBeTrue();
			typeof(ISupportRepository<Ticket, int>).GetTypeInfo().IsInstanceOfType(repo2).ShouldBeTrue();

			Assert.Throws<Exception>(
				() => repo1.Count()
			).Message.ShouldBe("can not get count!");

			Assert.Throws<Exception>(
				() => repo2.Count()
			).Message.ShouldBe("can not get count!");

			List<Ticket> activeTickets = repo1.GetActiveList();
			activeTickets.Count.ShouldBe(1);
			activeTickets[0].IsActive.ShouldBeTrue();

			activeTickets = repo2.GetActiveList();
			activeTickets.Count.ShouldBe(1);
			activeTickets[0].IsActive.ShouldBeTrue();
		}

		[Fact]
		public void Should_Get_DbContext()
		{
			var repository = The<IRepository<Post, Guid>>();

			repository.GetDbContext().ShouldBeOfType<BloggingDbContext>();
		}

		[Fact]
		public void Should_Get_DbContext_2()
		{
			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				The<IRepository<Blog>>().GetDbContext().ShouldBeOfType<BloggingDbContext>();

				uow.Complete();
			}
		}

		[Fact]
		public void Should_Get_DbContext_From_Second_DbContext()
		{
			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				The<IRepository<Ticket>>().GetDbContext().ShouldBeOfType<SupportDbContext>();

				uow.Complete();
			}
		}

		[Fact]
		public void Should_Get_DbContext_From_Second_DbContext_With_Custom_Repository()
		{
			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				The<ISupportRepository<Ticket>>().GetDbContext().ShouldBeOfType<SupportDbContext>();

				uow.Complete();
			}
		}
	}
}
