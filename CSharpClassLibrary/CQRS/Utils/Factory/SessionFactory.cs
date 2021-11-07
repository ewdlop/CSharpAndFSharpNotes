using CSharpClassLibrary.CQRS.Commands;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;

namespace CSharpClassLibrary.CQRS.Utils.Factory
{
    public sealed class SessionFactory
    {
        private readonly ISessionFactory _factory;

        public SessionFactory(CommandsConnectionString connectionString)
        {
            _factory = BuildSessionFactory(connectionString);
        }

        internal ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        private static ISessionFactory BuildSessionFactory(CommandsConnectionString connectionString)
        {
            //FluentConfiguration configuration = Fluently.Configure()
            //    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString.Value))
            //    .Mappings(m => m.FluentMappings
            //        .AddFromAssembly(Assembly.GetExecutingAssembly())
            //        .Conventions.Add(
            //            ForeignKey.EndsWith("ID"),
            //            ConventionBuilder.Property.When(criteria => criteria.Expect(x => x.Nullable, Is.Not.Set), x => x.Not.Nullable()))
            //        .Conventions.Add<OtherConversions>()
            //        .Conventions.Add<TableNameConvention>()
            //        .Conventions.Add<HiLoConvention>()
            //    );

            return null;//configuration.BuildSessionFactory();
        }
    }
}
