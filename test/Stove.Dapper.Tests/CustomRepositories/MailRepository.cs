using System;

using Dapper;

using Stove.Dapper.Repositories;
using Stove.Dapper.Tests.DbContexes;
using Stove.Dapper.Tests.Entities;
using Stove.Orm;
using Stove.Transactions;

namespace Stove.Dapper.Tests.CustomRepositories
{
    public class MailRepository : DapperEfRepositoryBase<MailDbContext, Mail, Guid>, IMailRepository
    {
        public MailRepository(IActiveTransactionProvider activeTransactionProvider) : base(activeTransactionProvider)
        {
        }

        public Mail GetMailById(Guid id)
        {
            return ActiveTransaction.Connection.QueryFirstOrDefault<Mail>("select top 1 * from Mails where Id = @id", new { id }, ActiveTransaction);
        }
    }
}
