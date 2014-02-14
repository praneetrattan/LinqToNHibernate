using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    using System.Linq.Expressions;

    using NHibernate;
    using NHibernate.Linq;

    public class Repository<TEntity> : IRepository<TEntity>
    {
        public ISession Session;


        public Repository(ISession session)
        {
            this.Session = session;
        }

        public IQueryable<TEntity> Get()
        {
            return this.Session.Query<TEntity>();
        }
    }
}
