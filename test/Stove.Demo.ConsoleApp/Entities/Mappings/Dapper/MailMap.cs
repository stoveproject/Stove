using DapperExtensions.Mapper;

namespace Stove.Demo.ConsoleApp.Entities.Mappings.Dapper
{
    public sealed class MailMap : ClassMapper<Mail>
    {
        public MailMap()
        {
            Table("Mails");
            Map(x => x.Id).Key(KeyType.Guid);
            AutoMap();
        }
    }
}
