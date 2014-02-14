namespace Persistence.EntityMaps
{
    using Domain.Entities;

    using FluentNHibernate.Mapping;

    public class BookMapping : ClassMap<Book>
    {
        public BookMapping()
        {
            this.Table("Books");
            this.Id(book => book.BookId, "BookId").Not.Nullable();
            this.Map(book => book.Title, "Title").Nullable();
            this.Map(book => book.Author, "Author").Nullable();
            this.Map(book => book.Abstract, "Abstract").Nullable();
            this.HasMany(fp => fp.Records).KeyColumn("Book");
        }
    }
}
