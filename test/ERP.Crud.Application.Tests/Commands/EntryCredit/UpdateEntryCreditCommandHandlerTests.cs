using ERP.Core.Interfaces;
using ERP.Crud.Application.Commands;
using ERP.Crud.Domain.Commands.EntryCredit;
using ERP.Crud.Domain.Commands.EntryCredit.Validators;
using ERP.Crud.Domain.Interfaces.Repositories;
using ERP.Domain.Events;
using Moq;
using Xunit;

namespace ERP.Crud.Application.Tests.Commands.EntryCredit
{
    public class UpdateEntryCreditCommandHandlerTests
    {
        private readonly UpdateEntryCreditCommandHandler _updateEntryCreditCommandHandler;
        private readonly Mock<IEntryCreditRepository> _entryCreditRepository;
        private readonly Mock<IEventPublisher<CreditEntryEvent>> _eventPublisher;

        public UpdateEntryCreditCommandHandlerTests()
        {
            _entryCreditRepository = new Mock<IEntryCreditRepository>();

            var updateCreditCommandValidator = new UpdateEntryCreditCommandValidator();

            _eventPublisher = new Mock<IEventPublisher<CreditEntryEvent>>();

            _updateEntryCreditCommandHandler = new UpdateEntryCreditCommandHandler(updateCreditCommandValidator, _entryCreditRepository.Object, _eventPublisher.Object);

            PrepareMockData();
        }

        [Fact]
        public void Should_Update_When_Command_Is_Valid()
        {
            var command = new UpdateEntryCreditCommand
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 1
            };

            _updateEntryCreditCommandHandler.Handle(command);

            _entryCreditRepository.Verify(r => r.Update(It.IsAny<Domain.Entities.EntryCredit>()), Times.Once);
        }

        [Fact]
        public void Should_Not_Update_When_Command_Is_Invalid()
        {
            var command = new UpdateEntryCreditCommand
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 0
            };

            _updateEntryCreditCommandHandler.Handle(command);

            _entryCreditRepository.Verify(r => r.Update(It.IsAny<Domain.Entities.EntryCredit>()), Times.Never);
        }

        private void PrepareMockData()
        {
            var entry = new Domain.Entities.EntryCredit
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 100
            };

            _entryCreditRepository.Setup(m => m.GetByIdAsync(entry.Id).Result).Returns(entry);
        }
    }
}