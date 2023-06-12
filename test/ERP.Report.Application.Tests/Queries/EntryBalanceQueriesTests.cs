using ERP.Report.Application.Queries;
using ERP.Report.Domain.Entitites;
using ERP.Report.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace ERP.Report.Application.Tests.Queries
{
    public class EntryBalanceQueriesTests
    {
        private readonly Mock<IEntryBalanceRepository> _entryBalanceRepository;
        private readonly IEntryBalanceQueries _entryBalanceQueries;

        public EntryBalanceQueriesTests()
        {
            _entryBalanceRepository = new Mock<IEntryBalanceRepository>();

            _entryBalanceQueries = new EntryBalanceQueries(_entryBalanceRepository.Object);

            PrepareMockData();
        }

        [Fact]
        public async void Should_Return_GetByDateAsync()
        {
            DateOnly dateRef = new(2000, 1, 1);

            var result = await _entryBalanceQueries.GetByDateAsync(dateRef);

            Assert.NotNull(result);
        }

        [Fact]
        public void Should_Return_Filter_DateRef_Invalid()
        {
            DateTime dateRef = new(1, 1, 1);

            var validation = EntryBalanceFiltersHelper.IsValidDateRef(dateRef);

            Assert.True(validation.Any());
        }

        [Fact]
        public void Should_Return_Filter_DateRef_Valid()
        {
            DateTime dateRef = new(2000, 1, 1);

            var validation = EntryBalanceFiltersHelper.IsValidDateRef(dateRef);

            Assert.True(!validation.Any());
        }

        [Fact]
        public void Should_Return_Filter_Period_Valid()
        {
            DateTime dateStart = new(2000, 1, 1);
            DateTime dateEnd = new(2000, 1, 10);

            var validation = EntryBalanceFiltersHelper.IsValidPeriod(dateStart, dateEnd);

            Assert.True(!validation.Any());
        }

        [Fact]
        public void Should_Return_Filter_Period_InValid()
        {
            DateTime dateStart = new(2000, 1, 1);
            DateTime dateEnd = new(1999, 1, 10);

            var validation = EntryBalanceFiltersHelper.IsValidPeriod(dateStart, dateEnd);

            Assert.True(validation.Any());
        }

        [Fact]
        public void Should_Return_Filter_Period_DateStart_InValid()
        {
            DateTime dateStart = new(1, 1, 1);
            DateTime dateEnd = new(1999, 1, 10);

            var validation = EntryBalanceFiltersHelper.IsValidPeriod(dateStart, dateEnd);

            Assert.True(validation.Any());
        }

        [Fact]
        public void Should_Return_Filter_Period_DateEnd_InValid()
        {
            DateTime dateStart = new(2022, 1, 1);
            DateTime dateEnd = new(1, 1, 1);

            var validation = EntryBalanceFiltersHelper.IsValidPeriod(dateStart, dateEnd);

            Assert.True(validation.Any());
        }

        [Fact]
        public async void Should_Return_GetByPeriodAsync()
        {
            DateOnly dateStart = new(2000, 1, 1);
            DateOnly dateEnd = new(2000, 1, 10);

            var result = await _entryBalanceQueries.GetByPeriodAsync(dateStart, dateEnd);

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() > 0);
        }

        private void PrepareMockData()
        {
            var entry_by_date = new EntryBalance
            {
                ReferenceDate = new(2000, 1, 1),
                Total = 100,
                TotalCredit = 100,
                TotalDebit = 200,
            };

            _entryBalanceRepository.Setup(m => m.GetByDateAsync(entry_by_date.ReferenceDate).Result).Returns(entry_by_date);

            DateOnly dateStart = new(2000, 1, 1);
            DateOnly dateEnd = new(2000, 1, 10);

            var list = new List<EntryBalance>();

            for (int i = 1; i <= 10; i++)
            {
                var entry = new EntryBalance
                {
                    ReferenceDate = new(2000, 1, i),
                    Total = 100,
                    TotalCredit = 100,
                    TotalDebit = 200,
                };
                list.Add(entry);
            }

            _entryBalanceRepository.Setup(m => m.GetByPeriodAsync(dateStart, dateEnd).Result).Returns(list);
        }
    }
}