using Autofac.Extras.IocManager;

using Stove.Couchbase.Couchbase.Repositories;
using Stove.Domain.Repositories;
using Stove.Reflection.Extensions;

namespace Stove.Couchbase.Couchbase
{
    public static class StoveCouchbaseRegistrationExtensions
    {
        public static IIocBuilder UseStoveCouchbase(IIocBuilder builder)
        {
            return builder
                .RegisterServices(r =>
                {
                    r.RegisterGeneric(typeof(IRepository<>), typeof(CouchbaseRepositoryBase<>));

                    r.RegisterAssemblyByConvention(typeof(StoveCouchbaseRegistrationExtensions).GetAssembly());
                });
        }
    }
}
