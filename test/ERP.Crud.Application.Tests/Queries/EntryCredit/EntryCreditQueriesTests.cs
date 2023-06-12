using ERP.Crud.Application.Queries;
using ERP.Crud.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace ERP.Crud.Application.Tests.Queries.EntryCredit
{
    public class EntryCreditQueriesTests
    {
        private readonly Mock<IEntryCreditRepository> _entryCreditRepository;
        private readonly IEntryCreditQueries _entryCreditQueries;

        public EntryCreditQueriesTests()
        {
            _entryCreditRepository = new Mock<IEntryCreditRepository>();

            _entryCreditQueries = new EntryCreditQueries(_entryCreditRepository.Object);

            PrepareMockData();
        }

        [Fact]
        public async void Should_Return_By_Id_1()
        {
            var result = await _entryCreditQueries.GetByIdAsync(1);

            Assert.NotNull(result);

            Assert.True(result.Value > 0);
        }

        [Fact]
        public async void Should_Not_Return_By_Id_2()
        {
            var result = await _entryCreditQueries.GetByIdAsync(2);

            Assert.Null(result);
        }

        [Fact]
        public async void Should_Return_All_With_10_Rows()
        {
            var result = await _entryCreditQueries.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 10);
        }

        private void PrepareMockData()
        {
            var entry_by_Id = new Domain.Entities.EntryCredit
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 100
            };

            _entryCreditRepository.Setup(m => m.GetByIdAsync(entry_by_Id.Id).Result).Returns(entry_by_Id);

            var list = new List<Domain.Entities.EntryCredit>();

            for (int i = 1; i <= 10; i++)
            {
                var entry = new Domain.Entities.EntryCredit
                {
                    Id = 1,
                    DateOfIssue = DateTime.Now,
                    Description = "Unit Test",
                    Value = 100
                };
                list.Add(entry);
            }

            _entryCreditRepository.Setup(m => m.GetAllAsync().Result).Returns(list);
        }
    }
}