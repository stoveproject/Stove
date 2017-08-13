using System;

using Stove.Domain.Repositories;
using Stove.EntityFrameworkCore.Tests.Domain;

namespace Stove.EntityFrameworkCore.Tests.Ef
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
    }
}