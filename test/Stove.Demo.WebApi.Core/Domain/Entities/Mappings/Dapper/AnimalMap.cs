using DapperExtensions.Mapper;

namespace Stove.Demo.WebApi.Core.Domain.Entities.Mappings.Dapper
{
    public sealed class AnimalMap : ClassMapper<Animal>
    {
        public AnimalMap()
        {
            Table("Animals");
			Map(x => x.Id).Key(KeyType.Identity);
			AutoMap();
        }
    }
}
