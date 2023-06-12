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
    public class DeleteEntryCreditCommandHandlerTests
    {
        private readonly DeleteEntryCreditCommandHandler _deleteEntryCreditCommandHandler;
        private readonly Mock<IEntryCreditRepository> _entryCreditRepository;
        private readonly Mock<IEventPublisher<CreditEntryEvent>> _eventPublisher;

        public DeleteEntryCreditCommandHandlerTests()
        {
            _entryCreditRepository = new Mock<IEntryCreditRepository>();

            var deleteCreditCommandValidator = new DeleteEntryCreditCommandValidator();

            _eventPublisher = new Mock<IEventPublisher<CreditEntryEvent>>();

            _deleteEntryCreditCommandHandler = new DeleteEntryCreditCommandHandler(deleteCreditCommandValidator, _entryCreditRepository.Object, _eventPublisher.Object);

            PrepareMockData();
        }

        [Fact]
        public void Should_Delete_When_Command_Is_Valid()
        {
            var command = new DeleteEntryCreditCommand
            {
                Id = 1,
            };

            _deleteEntryCreditCommandHandler.Handle(command);

            _entryCreditRepository.Verify(r => r.Delete(It.IsAny<Domain.Entities.EntryCredit>()), Times.Once);
        }

        [Fact]
        public void Should_Not_Update_When_Command_Is_Invalid()
        {
            var command = new DeleteEntryCreditCommand
            {
                Id = -1,
            };

            _deleteEntryCreditCommandHandler.Handle(command);

            _entryCreditRepository.Verify(r => r.Delete(It.IsAny<Domain.Entities.EntryCredit>()), Times.Never);
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