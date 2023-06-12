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
    public class DeleteEntryDebitCommandHandlerTests
    {
        private readonly DeleteEntryDebitCommandHandler _deleteEntryDebitCommandHandler;
        private readonly Mock<IEntryDebitRepository> _entryDebitRepository;
        private readonly Mock<IEventPublisher<DebitEntryEvent>> _eventPublisher;

        public DeleteEntryDebitCommandHandlerTests()
        {
            _entryDebitRepository = new Mock<IEntryDebitRepository>();

            var deleteDebitCommandValidator = new DeleteEntryDebitCommandValidator();

            _eventPublisher = new Mock<IEventPublisher<DebitEntryEvent>>();

            _deleteEntryDebitCommandHandler = new DeleteEntryDebitCommandHandler(deleteDebitCommandValidator, _entryDebitRepository.Object, _eventPublisher.Object);

            PrepareMockData();
        }

        [Fact]
        public void Should_Delete_When_Command_Is_Valid()
        {
            var command = new DeleteEntryDebitCommand
            {
                Id = 1,
            };

            _deleteEntryDebitCommandHandler.Handle(command);

            _entryDebitRepository.Verify(r => r.Delete(It.IsAny<Domain.Entities.EntryDebit>()), Times.Once);
        }

        [Fact]
        public void Should_Not_Update_When_Command_Is_Invalid()
        {
            var command = new DeleteEntryDebitCommand
            {
                Id = -1,
            };

            _deleteEntryDebitCommandHandler.Handle(command);

            _entryDebitRepository.Verify(r => r.Delete(It.IsAny<Domain.Entities.EntryDebit>()), Times.Never);
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