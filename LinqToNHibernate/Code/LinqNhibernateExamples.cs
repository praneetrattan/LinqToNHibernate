using System;
using System.Collections.Generic;
using System.Linq;

namespace Code
{
    using DatabaseGenerator;

    using Domain.Entities;

    using NUnit.Framework;

    using Persistence;

    using StructureMap;

    [TestFixture]
    internal class LinqNhibernateExamples
    {
        private IRepository<Book> bookRepository;

        private IRepository<RecordKeeping> recordKeepingRepository;

        private IRepository<Member> memberRepository;

        [TestFixtureSetUp]
        public void Setup()
        {
            BootStrapper.BootStrap();
            bookRepository = ObjectFactory.GetInstance<IRepository<Book>>();
            recordKeepingRepository = ObjectFactory.GetInstance<IRepository<RecordKeeping>>();
            memberRepository = ObjectFactory.GetInstance<IRepository<Member>>();
        }

        /* Run this to setup the test database only. Not an actual test. */
        [Test]
        public void SetupDatabase()
        {
            SampleDataGenerator.SetupTestDatabase();
        }

        [Test]
        public void CountVsAny()
        {
            bool doesTableBooksHaveAnyData = false;

            /* Count */
            if (bookRepository.Get().Count(book => book.Author != null) > 0)
            {
                doesTableBooksHaveAnyData = true;
            }

            /* Any */
            if (bookRepository.Get().Any(book => book.Author != null))
            {
                doesTableBooksHaveAnyData = true;
            }

            Assert.True(doesTableBooksHaveAnyData);
        }

        [Test]
        public void ProjectionVsFullRetrival()
        {

            /* Projection */
            var projectedBookTitles = bookRepository.Get().Select(b => b.Title).ToList();
            Assert.NotNull(projectedBookTitles);


            /* Full Retrival */
            var allBooks = bookRepository.Get().ToList();
            // Some code
            var allBookTitles = allBooks.Select(b => b.Title);
            Assert.NotNull(allBookTitles);
        }

        [Test]
        public void UsingToList()
        {
            /* Case 1 - Retriving objects multiple times (Unfavorable) */
            var bookTitles = bookRepository.Get().Select(b => b.Title).ToList();

            var bookAuthors = bookRepository.Get().Select(b => b.Author).ToList();

            var bookAbstract = bookRepository.Get().Select(b => b.Abstract).ToList();



            /* Case 2 - Retriving once, but prematurely (Not always favorable) */
            var bookDetails = bookRepository.Get().Select(b => new { b.Title, b.Author, b.Abstract }).ToList();
            //Huge chunk of code involving high use of memory
            //Use of bookDetails
            var book



            /* Case 2 - Deffered execution (favorable) */
            var defferedBookDetails = bookRepository.Get().Select(b => new { b.Title, b.Author, b.Abstract });
            //Huge chunk of code involving high use of memory
            var listedDefferedBookDetails = defferedBookDetails.ToList();



            /* Case 3  - Retriving once, but before utmost use (favorable) */
            var bookInfo = bookRepository.Get().Select(b => new { b.Title, b.Author, b.Abstract }).ToList();

        }

        [Test]
        public void SelectManyBad()
        {
            var members = this.memberRepository.Get();

            var records = new List<Tuple<string, string>>();

            foreach (Member member in members)
            {
                foreach (var record in member.Records)
                {
                    records.Add(Tuple.Create(member.Name, record.CheckoutDateTime));
                }
            }
        }

        [Test]
        public void SelectManyGood()
        {
            var results1 =
                this.memberRepository.Get()
                    .SelectMany(m => m.Records, (m, r) => Tuple.Create(m.Name, r.CheckoutDateTime))
                    .ToList();

            var results2 =
                this.recordKeepingRepository.Get().Select(r => Tuple.Create(r.Member.Name, r.CheckoutDateTime)).ToList();

            var result3 =
                this.memberRepository.Get()
                    .Join(
                        this.recordKeepingRepository.Get(),
                        m => m.MemberId,
                        r => r.Member.MemberId,
                        (m, r) => Tuple.Create(m.Name, r.CheckoutDateTime))
                    .ToList();
        }
    }
}
