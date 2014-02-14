using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Member
    {
        public virtual int MemberId { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsPremiumMember { get; set; }

        public virtual IEnumerable<RecordKeeping> Records { get; set; }
    }
}
