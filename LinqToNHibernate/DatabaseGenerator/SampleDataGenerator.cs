namespace DatabaseGenerator
{
    using System;

    public class SampleDataGenerator
    {
        public const string DatabaseName = "KnowledgeBase";

        private static SqlHelper sqlHelper;

        private const string DummyAbstract = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

        public static void SetupTestDatabase()
        {
            sqlHelper = new SqlHelper();
            sqlHelper.StartConnection(null);
            CreateDatabase();
            CreateSchema();
            InsertData();
        }

        private static void CreateDatabase()
        {
            var sql = @"IF(NOT EXISTS (SELECT * FROM sys.databases WHERE name like '"+ DatabaseName + @"'))
                        CREATE DATABASE " + DatabaseName + ";";
            sqlHelper.ExecuteNonQuery(sql);
        }

        private static void CreateSchema()
        {
            var sql = @"USE KnowledgeBase

                        IF OBJECT_ID('dbo.RecordKeeping', 'U') IS NOT NULL
                          DROP TABLE dbo.RecordKeeping

                        IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL
                          DROP TABLE dbo.Books

                        IF OBJECT_ID('dbo.Members', 'U') IS NOT NULL
                          DROP TABLE dbo.Members
                        
                        /* Creating Schema */
                        CREATE TABLE Books(
                        BookId int PRIMARY KEY,         /* Not using identity column to easily insert data */
                        Title varchar(50) NOT NULL,
                        Author varchar (50),
                        Abstract varchar (2000))

                        CREATE TABLE Members(
                        MemberId int PRIMARY KEY,
                        Name varchar(50) NOT NULL,
                        IsPremiumMember bit NOT NULL)

                        CREATE TABLE RecordKeeping(
                        RecordId int PRIMARY KEY,
                        Member int NOT NULL,
                        Book int NOT NULL,
                        CheckoutDateTime datetime,
                        FOREIGN KEY (Member) REFERENCES Members(MemberId),
                        FOREIGN KEY (Book) REFERENCES Books(BookId))
                        ";
            sqlHelper.ExecuteNonQuery(sql);
        }

        private static void InsertData()
        {
            //Books
            InsertRowInBooks(1, "Book Title 1", "Author A", "This is a long abstract for Book 1, written by Author A" + DummyAbstract);
            InsertRowInBooks(2, "Book Title 2", "Author B", "This is a long abstract for Book 2, written by Author B" + DummyAbstract);
            InsertRowInBooks(3, "Book Title 3", "Author A", "This is a long abstract for Book 3, written by Author A" + DummyAbstract);
            InsertRowInBooks(4, "Book Title 4", "Author C", "This is a long abstract for Book 4, written by Author C" + DummyAbstract); //Not in RecordKeeping

            //Adding more records
            for (int i = 5; i <= 500; i++)
            {
                InsertRowInBooks(i, "Book Title " + i, "Author A", "This is a long abstract for Book " + i + ", written by Author A" + DummyAbstract);
            }

            //Members
            InsertRowInMembers(1, "Member P", "1");
            InsertRowInMembers(2, "Member Q", "0");
            InsertRowInMembers(3, "Member R", "0");
            InsertRowInMembers(4, "Member S", "1");
            InsertRowInMembers(5, "Member T", "0"); //Not in RecordKeeping

            //RecordKeeping
            InsertRowInRecordKeeping(1, 1, 1, DateTime.Now);
            InsertRowInRecordKeeping(2, 1, 1, DateTime.Now);
            InsertRowInRecordKeeping(3, 2, 2, DateTime.Now);
            InsertRowInRecordKeeping(4, 3, 1, DateTime.Now);
            InsertRowInRecordKeeping(5, 1, 1, DateTime.Now);
            InsertRowInRecordKeeping(6, 1, 3, DateTime.Now);
            InsertRowInRecordKeeping(7, 4, 1, DateTime.Now);
        }

        private static void InsertRowInBooks(int bookId, string title, string author, string bookAbstract)
        {
            var sql = @"INSERT INTO Books VALUES (" + bookId + ", '" + title + "', '" + author + "', '" + bookAbstract + "') ";
            sqlHelper.ExecuteNonQuery(sql);
        }

        private static void InsertRowInMembers(int memberId, string name, string isPremiumMember)
        {
            var sql = @"INSERT INTO Members VALUES (" + memberId + ", '" + name + "', " + isPremiumMember + ") ";
            sqlHelper.ExecuteNonQuery(sql);
        }

        private static void InsertRowInRecordKeeping(int recordId, int memberId, int bookId, DateTime checkoutDatetime)
        {
            var sql = @"INSERT INTO RecordKeeping VALUES (" + recordId + ", " + memberId + ", " + bookId + ", CONVERT( datetime, '" + checkoutDatetime + "')) ";
            sqlHelper.ExecuteNonQuery(sql);
        }
    }
}
