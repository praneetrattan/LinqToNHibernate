namespace Persistence
{
    using System;

    using DatabaseGenerator;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using NHibernate;

    using Persistence.EntityMaps;

    using StructureMap;

    public static class BootStrapper
    {
        private const string TimeoutProperty = "command_timeout";

        private const int DefaultTimeout = 0;

        public static void BootStrap()
        {
            const string ConnectionString = SqlHelper.ConnectionString;

            MsSqlConfiguration msSqlConfiguration = MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString);
            try
            {
                ObjectFactory.Initialize(
                    x =>
                        {
                            x.For<IInterceptor>().Use<DependencyInjectionEntityInterceptor>();
                            x.For(typeof(IRepository<>)).Use(typeof(Repository<>));

                            x.ForSingletonOf<ISessionFactory>()
                             .Use(
                                 y =>
                                 Fluently.Configure()
                                         .Database(msSqlConfiguration)
                                         .ExposeConfiguration(
                                             configuration =>
                                             configuration.SetProperty(TimeoutProperty, DefaultTimeout.ToString()))
                                         .ExposeConfiguration(
                                             configuration =>
                                             configuration.SetInterceptor(y.GetInstance<IInterceptor>()))
                                         .Mappings(
                                             m =>
                                             m.FluentMappings.AddFromAssemblyOf<BookMapping>()
                                              .Conventions.AddFromAssemblyOf<BookMapping>())
                                         .BuildSessionFactory());

                            x.For<ISession>()
                             .HybridHttpOrThreadLocalScoped()
                             .Use(y => y.GetInstance<ISessionFactory>().OpenSession());
                        });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
