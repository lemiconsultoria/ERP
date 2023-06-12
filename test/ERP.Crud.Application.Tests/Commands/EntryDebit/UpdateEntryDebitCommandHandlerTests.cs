using ERP.Core.Interfaces;
using ERP.Crud.Application.Commands;
using ERP.Crud.Domain.Commands.EntryDebit;
using ERP.Crud.Domain.Commands.EntryDebit.Validators;
using ERP.Crud.Domain.Interfaces.Repositories;
using ERP.Domain.Events;
using Moq;
using Xunit;

namespace ERP.Crud.Application.Tests.Commands.EntryDebit
{
    public class UpdateEntryDebitCommandHandlerTests
    {
        private readonly UpdateEntryDebitCommandHandler _updateEntryDebitCommandHandler;
        private readonly Mock<IEntryDebitRepository> _entryDebitRepository;
        private readonly Mock<IEventPublisher<DebitEntryEvent>> _eventPublisher;

        public UpdateEntryDebitCommandHandlerTests()
        {
            _entryDebitRepository = new Mock<IEntryDebitRepository>();

            var updateDebitCommandValidator = new UpdateEntryDebitCommandValidator();

            _eventPublisher = new Mock<IEventPublisher<DebitEntryEvent>>();

            _updateEntryDebitCommandHandler = new UpdateEntryDebitCommandHandler(updateDebitCommandValidator, _entryDebitRepository.Object, _eventPublisher.Object);

            PrepareMockData();
        }

        [Fact]
        public void Should_Update_When_Command_Is_Valid()
        {
            var command = new UpdateEntryDebitCommand
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 1
            };

            _updateEntryDebitCommandHandler.Handle(command);

            _entryDebitRepository.Verify(r => r.Update(It.IsAny<Domain.Entities.EntryDebit>()), Times.Once);
        }

        [Fact]
        public void Should_Not_Update_When_Command_Is_Invalid()
        {
            var command = new UpdateEntryDebitCommand
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 0
            };

            _updateEntryDebitCommandHandler.Handle(command);

            _entryDebitRepository.Verify(r => r.Update(It.IsAny<Domain.Entities.EntryDebit>()), Times.Never);
        }

        private void PrepareMockData()
        {
            var entry = new Domain.Entities.EntryDebit
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 100
            };

            _entryDebitRepository.Setup(m => m.GetByIdAsync(entry.Id).Result).Returns(entry);
        }
    }
}