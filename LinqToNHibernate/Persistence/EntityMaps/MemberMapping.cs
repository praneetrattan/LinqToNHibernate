using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityMaps
{
    using Domain.Entities;

    using FluentNHibernate.Mapping;

    public class MemberMapping : ClassMap<Member>
    {
        public MemberMapping()
        {
            this.Table("Members");
            this.Id(m => m.MemberId, "MemberId").Not.Nullable();
            this.Map(m => m.IsPremiumMember, "IsPremiumMember").Nullable();
            this.Map(m => m.Name, "Name").Nullable();
            this.HasMany(fp => fp.Records).KeyColumn("Member");
        }
        

    }
}
