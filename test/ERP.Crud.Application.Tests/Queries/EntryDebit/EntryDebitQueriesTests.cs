using ERP.Crud.Application.Queries;
using ERP.Crud.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace ERP.Crud.Application.Tests.Queries.EntryDebit
{
    public class EntryDebitQueriesTests
    {
        private readonly Mock<IEntryDebitRepository> _entryDebitRepository;
        private readonly IEntryDebitQueries _entryDebitQueries;

        public EntryDebitQueriesTests()
        {
            _entryDebitRepository = new Mock<IEntryDebitRepository>();

            _entryDebitQueries = new EntryDebitQueries(_entryDebitRepository.Object);

            PrepareMockData();
        }

        [Fact]
        public async void Should_Return_By_Id_1()
        {
            var result = await _entryDebitQueries.GetByIdAsync(1);

            Assert.NotNull(result);

            Assert.True(result.Value > 0);
        }

        [Fact]
        public async void Should_Not_Return_By_Id_2()
        {
            var result = await _entryDebitQueries.GetByIdAsync(2);

            Assert.Null(result);
        }

        [Fact]
        public async void Should_Return_All_With_10_Rows()
        {
            var result = await _entryDebitQueries.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 10);
        }

        private void PrepareMockData()
        {
            var entry_by_Id = new Domain.Entities.EntryDebit
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 100
            };

            _entryDebitRepository.Setup(m => m.GetByIdAsync(entry_by_Id.Id).Result).Returns(entry_by_Id);

            var list = new List<Domain.Entities.EntryDebit>();

            for (int i = 1; i <= 10; i++)
            {
                var entry = new Domain.Entities.EntryDebit
                {
                    Id = 1,
                    DateOfIssue = DateTime.Now,
                    Description = "Unit Test",
                    Value = 100
                };
                list.Add(entry);
            }

            _entryDebitRepository.Setup(m => m.GetAllAsync().Result).Returns(list);
        }
    }
}