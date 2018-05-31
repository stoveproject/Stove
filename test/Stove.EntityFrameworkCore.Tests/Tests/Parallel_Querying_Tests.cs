using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using Stove.Application.Services;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Tests.Domain;

using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
	public class Parallel_Querying_Tests : EntityFrameworkCoreTestBase
	{
		private readonly IParallelQueryExecuteDemo _parallelQueryExecuteDemo;

		public Parallel_Querying_Tests()
		{
			Building(builder => { }).Ok();

			_parallelQueryExecuteDemo = The<IParallelQueryExecuteDemo>();
		}

		[Fact(Skip = "Sqlite does not support nested transactions")]
		public async Task Should_Run_Parallel_With_Different_UnitOfWorks()
		{
			await _parallelQueryExecuteDemo.RunAsync();
		}
	}

	public interface IParallelQueryExecuteDemo
	{
		Task<int> GetBlogCountAsync();

		Task RunAsync();
	}

	public class ParallelQueryExecuteDemo : IApplicationService, IParallelQueryExecuteDemo
	{
		private readonly IRepository<Blog> _blogRepository;

		public ParallelQueryExecuteDemo(IRepository<Blog> blogRepository)
		{
			_blogRepository = blogRepository;
		}

		[UnitOfWork]
		public virtual async Task RunAsync()
		{
			const int threadCount = 32;

			var tasks = new List<Task<int>>();

			for (var i = 0; i < threadCount; i++)
			{
				tasks.Add(GetBlogCountAsync());
			}

			await Task.WhenAll(tasks.Cast<Task>().ToArray());

			foreach (Task<int> task in tasks)
			{
				task.Result.ShouldBeGreaterThan(0);
			}
		}

		[UnitOfWork(TransactionScopeOption.RequiresNew, false)]
		public virtual async Task<int> GetBlogCountAsync()
		{
			await Task.Delay(RandomHelper.GetRandom(0, 100));
			int result = await _blogRepository.GetAll().CountAsync();
			await Task.Delay(RandomHelper.GetRandom(0, 100));
			return result;
		}
	}
}
