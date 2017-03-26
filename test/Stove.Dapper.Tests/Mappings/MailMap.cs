using DapperExtensions.Mapper;

using Stove.Dapper.Tests.Entities;

namespace Stove.Dapper.Tests.Mappings
{
    public sealed class MailMap : ClassMapper<Mail>
    {
        public MailMap()
        {
            Table("Mails");
            AutoMap();
        }
    }
}
