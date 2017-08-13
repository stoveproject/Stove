using DapperExtensions.Mapper;

namespace Stove.Demo.WebApi.Core.Domain.Entities.Mappings.Dapper
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
