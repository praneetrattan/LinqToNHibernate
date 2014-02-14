using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RecordKeeping
    {
        public virtual int RecordId { get; set; }
        public virtual Member Member { get; set; }
        public virtual Book Book { get; set; }
        public virtual int BookId { get; set; }
        public virtual string CheckoutDateTime { get; set; }
    }
}
