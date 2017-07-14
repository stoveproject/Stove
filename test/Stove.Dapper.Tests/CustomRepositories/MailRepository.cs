using System;

using Dapper;

using Stove.Dapper.Repositories;
using Stove.Dapper.Tests.DbContexes;
using Stove.Dapper.Tests.Entities;
using Stove.Orm;
using Stove.Data;

namespace Stove.Dapper.Tests.CustomRepositories
{
    public class MailRepository : DapperEfRepositoryBase<MailDbContext, Mail, Guid>, IMailRepository
    {
        public MailRepository(IActiveTransactionProvider activeTransactionProvider) : base(activeTransactionProvider)
        {
        }

        public Mail GetMailById(Guid id)
        {
            return ActiveTransaction.Connection.QueryFirstOrDefault<Mail>("select  * from Mails where Id = @id limit 1", new { id }, ActiveTransaction);
        }
    }
}
