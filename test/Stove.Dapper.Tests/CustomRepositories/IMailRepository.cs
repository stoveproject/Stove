using System;

using Stove.Dapper.Repositories;
using Stove.Dapper.Tests.Entities;

namespace Stove.Dapper.Tests.CustomRepositories
{
    public interface IMailRepository : IDapperRepository<Mail, Guid>
    {
        Mail GetMailById(Guid id);
    }
}
