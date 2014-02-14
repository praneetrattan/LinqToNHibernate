using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityMaps
{
    using Domain.Entities;

    using FluentNHibernate.Mapping;

    public class RecordKeepingMapping : ClassMap<RecordKeeping>
    {
        public RecordKeepingMapping()
        {
            this.Table("RecordKeeping");
            this.Id(record => record.RecordId, "RecordId").Not.Nullable();
            this.Map(record => record.CheckoutDateTime, "CheckoutDateTime").Nullable();
            this.Map(record => record.BookId, "Book").Nullable();

            this.References(fp => fp.Book, "Book");
            this.References(fp => fp.Member, "Member");
        }
        
    }
}
