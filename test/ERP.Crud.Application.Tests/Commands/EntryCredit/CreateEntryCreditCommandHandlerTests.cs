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
    public class CreateEntryCreditCommandHandlerTests
    {
        private readonly CreateEntryCreditCommandHandler _createEntryCreditCommandHandler;
        private readonly Mock<IEntryCreditRepository> _entryCreditRepository;
        private readonly Mock<IEventPublisher<CreditEntryEvent>> _eventPublisher;

        public CreateEntryCreditCommandHandlerTests()
        {
            _entryCreditRepository = new Mock<IEntryCreditRepository>();

            var createCreditCommandValidator = new CreateEntryCreditCommandValidator();

            _eventPublisher = new Mock<IEventPublisher<CreditEntryEvent>>();

            _createEntryCreditCommandHandler = new CreateEntryCreditCommandHandler(createCreditCommandValidator, _entryCreditRepository.Object, _eventPublisher.Object);
        }

        [Fact]
        public void Should_Create_When_Command_Is_Valid()
        {
            var command = new CreateEntryCreditCommand
            {
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 1
            };

            _createEntryCreditCommandHandler.Handle(command);

            _entryCreditRepository.Verify(r => r.Add(It.IsAny<Domain.Entities.EntryCredit>()), Times.Once);
            _eventPublisher.Verify(p => p.Publish(It.IsAny<CreditEntryEvent>()), Times.Once);
        }

        [Fact]
        public void Should_Not_Create_When_Command_Is_Invalid()
        {
            var command = new CreateEntryCreditCommand
            {
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 0
            };

            _createEntryCreditCommandHandler.Handle(command);

            _entryCreditRepository.Verify(r => r.Add(It.IsAny<Domain.Entities.EntryCredit>()), Times.Never);
            _eventPublisher.Verify(p => p.Publish(It.IsAny<CreditEntryEvent>()), Times.Never);
        }
    }
}