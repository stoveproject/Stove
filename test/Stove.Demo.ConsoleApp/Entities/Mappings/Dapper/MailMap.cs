using DapperExtensions.Mapper;

namespace Stove.Demo.ConsoleApp.Entities.Mappings.Dapper
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
