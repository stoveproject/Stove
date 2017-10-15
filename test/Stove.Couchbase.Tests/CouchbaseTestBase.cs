using System;
using System.Collections.Generic;
using System.Reflection;

using Couchbase.Configuration.Client;

using Stove.TestBase;

namespace Stove.Couchbase.Tests
{
    public abstract class CouchbaseTestBase : ApplicationTestBase<CouchbaseTestBootstrapper>
    {
        protected CouchbaseTestBase() : base(true)
        {
            Building(builder =>
            {
                builder
                    .UseStoveCouchbase(configuration =>
                    {
                        ClientConfiguration cfg = configuration.ClientConfiguration;
                        cfg.Servers.Add(new Uri("http://127.0.0.1:8091/pools"));
                        cfg.UseSsl = false;
                        cfg.BucketConfigs = new Dictionary<string, BucketConfiguration>
                        {
                            {
                                "default", new BucketConfiguration
                                {
                                    BucketName = "default",
                                    UseSsl = false,
                                    Password = "",
                                    DefaultOperationLifespan = 2000,
                                    PoolConfiguration = new PoolConfiguration
                                    {
                                        MaxSize = 10,
                                        MinSize = 5,
                                        SendTimeout = 12000
                                    }
                                }
                            }
                        };

                        return configuration;
                    })
                    .RegisterServices(r => { r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()); });
            });
        }
    }
}
