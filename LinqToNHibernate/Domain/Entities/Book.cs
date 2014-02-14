namespace Domain.Entities
{
    using System.Collections.Generic;

    public class Book
    {
        public virtual int BookId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Author { get; set; }
        public virtual string Abstract { get; set; }
        public virtual IEnumerable<RecordKeeping> Records { get; set; }
    }
}
