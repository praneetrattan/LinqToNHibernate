using System;
using System.Linq;

namespace Persistence
{
    using NHibernate;

    using StructureMap;

    public class DependencyInjectionEntityInterceptor : EmptyInterceptor
    {
        private readonly TypeResolver typeResolver;

        private ISession session;


        public DependencyInjectionEntityInterceptor(TypeResolver typeResolver)
        {
            this.typeResolver = typeResolver;
        }


        public override void SetSession(ISession sqlSession)
        {
            this.session = sqlSession;
        }


        public override object Instantiate(string clazz, EntityMode entityMode, object id)
        {
            if (entityMode == EntityMode.Poco)
            {
                Type type = this.typeResolver.ResolveType(clazz);

                if (type != null)
                {
                    if (type.GetConstructors().Any(constructor => constructor.GetParameters().Any()))
                    {
                        try
                        {
                            object instance = ObjectFactory.GetInstance(type);

                            this.session.SessionFactory
                                    .GetClassMetadata(clazz)
                                    .SetIdentifier(instance, id, entityMode);
                            return instance;
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }
                }
            }

            return base.Instantiate(clazz, entityMode, id);
        }
    }
}
