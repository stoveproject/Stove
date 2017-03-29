using DapperExtensions.Mapper;

namespace Stove.Demo.ConsoleApp.Entities.Mappings.Dapper
{
    public sealed class AnimalMap : ClassMapper<Animal>
    {
        public AnimalMap()
        {
            Table("Animals");
            AutoMap();
        }
    }
}
