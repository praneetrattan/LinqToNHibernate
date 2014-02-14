using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    using System.Linq.Expressions;
   
        public interface IRepository<TEntity>
        {
            IQueryable<TEntity> Get();
        }
    
}
