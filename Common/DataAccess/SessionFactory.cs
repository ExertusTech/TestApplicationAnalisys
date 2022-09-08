using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate;

namespace Common.DataAccess
{
    public class SessionFactory
    {
        private readonly ISessionFactory _factory;
        public bool Connected { get; }

        public SessionFactory(string connectionString)
        {
            try
            {
                _factory = BuildSessionFactory(connectionString);
                this.Connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.InnerException?.Message);
                throw;
            }
        }

        public ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        private static ISessionFactory BuildSessionFactory(string connectionString)
        {
            var configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add(
                        ForeignKey.EndsWith("Id"),
                        ConventionBuilder.Property
                            .When(criteria => criteria.Expect(x => x.Nullable, Is.Not.Set), x => x.Not.Nullable()))
                    .Conventions.Add<TableNameConvention>()
                    .Conventions.Add<CustomConvention>()
                )
                .Cache(c => c
                    .ProviderClass(typeof(NHibernate.Caches.CoreMemoryCache.CoreMemoryCacheProvider)
                        .AssemblyQualifiedName)
                    .UseSecondLevelCache()
                    .UseQueryCache()
                )
                .ExposeConfiguration(x => { x.Cache(e => e.DefaultExpiration = 1800); });

            return configuration.BuildSessionFactory();
        }

        public class TableNameConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table(instance.EntityType.Name);
            }
        }

        public class CustomConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.GeneratedBy.Increment();
            }
        }
    }
}