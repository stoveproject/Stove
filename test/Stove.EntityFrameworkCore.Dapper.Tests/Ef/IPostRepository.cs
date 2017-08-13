using System;

using Stove.Domain.Repositories;
using Stove.EntityFrameworkCore.Dapper.Tests.Domain;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Ef
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
    }
}