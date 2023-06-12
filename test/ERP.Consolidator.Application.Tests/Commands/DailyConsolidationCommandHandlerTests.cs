using ERP.Consolidator.Application.Commands;
using ERP.Consolidator.Domain.Commands.EntryBalance;
using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Core.Interfaces;
using Moq;
using Xunit;

namespace ERP.Consolidator.Application.Tests.Commands
{
    public class DailyConsolidationCommandHandlerTests
    {
        private readonly ICommandHandler<ConsolidateCommand, ConsolidateCommandResult> _consolidateCommandHandler;
        private readonly Mock<IEntryCreditRepository> _entryCreditRepository;
        private readonly Mock<IEntryDebitRepository> _entryDebitRepository;
        private readonly Mock<IDateToProcessRepository> _dateToProcessDebitRepository;
        private readonly Mock<IEntryBalanceRepository> _entryBalanceRepository;

        public DailyConsolidationCommandHandlerTests()
        {
            _entryCreditRepository = new Mock<IEntryCreditRepository>();
            _entryDebitRepository = new Mock<IEntryDebitRepository>();
            _dateToProcessDebitRepository = new Mock<IDateToProcessRepository>();
            _entryBalanceRepository = new Mock<IEntryBalanceRepository>();

            _consolidateCommandHandler = new DailyConsolidationCommandHandler(_entryCreditRepository.Object, _entryDebitRepository.Object, _dateToProcessDebitRepository.Object, _entryBalanceRepository.Object);
        }

        [Fact]
        public void Should_Execute_Command_With_OnlyToday_Add_With_Success()
        {
            var command = new ConsolidateCommand
            {
                OnlyToday = true
            };

            var result = _consolidateCommandHandler.Handle(command);

            Assert.True(result.Success);

            Assert.NotNull(result.Data);

            Assert.True(result.Data.DatesProcessed.Count > 0);

            _entryBalanceRepository.Verify(r => r.Add(It.IsAny<Domain.Entities.EntryBalance>()), Times.Once);
        }

        [Fact]
        public void Should_Execute_Command_With_OnlyToday_Update_With_Success()
        {
            var entry = new EntryBalance { ReferenceDate = DateOnly.FromDateTime(DateTime.Now) };

            _entryBalanceRepository.Setup(m => m.GetByDate(entry.ReferenceDate)).Returns(entry);

            var command = new ConsolidateCommand
            {
                OnlyToday = true
            };

            var result = _consolidateCommandHandler.Handle(command);

            Assert.True(result.Success);

            Assert.NotNull(result.Data);

            Assert.True(result.Data.DatesProcessed.Count > 0);

            _entryBalanceRepository.Verify(r => r.Update(It.IsAny<Domain.Entities.EntryBalance>()), Times.Once);
        }

        [Fact]
        public void Should_Execute_Command_With_OnlyToday_false_With_InSuccess()
        {
            var command = new ConsolidateCommand
            {
                OnlyToday = false
            };

            var result = _consolidateCommandHandler.Handle(command);

            Assert.True(!result.Success);
            _entryBalanceRepository.Verify(r => r.Add(It.IsAny<Domain.Entities.EntryBalance>()), Times.Never);
        }
    }
}