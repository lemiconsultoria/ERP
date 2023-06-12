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
    public class CreateEntryDebitCommandHandlerTests
    {
        private readonly CreateEntryDebitCommandHandler _createEntryDebitCommandHandler;
        private readonly Mock<IEntryDebitRepository> _entryDebitRepository;
        private readonly Mock<IEventPublisher<DebitEntryEvent>> _eventPublisher;

        public CreateEntryDebitCommandHandlerTests()
        {
            _entryDebitRepository = new Mock<IEntryDebitRepository>();

            var createDebitCommandValidator = new CreateEntryDebitCommandValidator();

            _eventPublisher = new Mock<IEventPublisher<DebitEntryEvent>>();

            _createEntryDebitCommandHandler = new CreateEntryDebitCommandHandler(createDebitCommandValidator, _entryDebitRepository.Object, _eventPublisher.Object);
        }

        [Fact]
        public void Should_Create_When_Command_Is_Valid()
        {
            var command = new CreateEntryDebitCommand
            {
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 1
            };

            _createEntryDebitCommandHandler.Handle(command);

            _entryDebitRepository.Verify(r => r.Add(It.IsAny<Domain.Entities.EntryDebit>()), Times.Once);
            _entryDebitRepository.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Should_Not_Create_When_Command_Is_Invalid()
        {
            var command = new CreateEntryDebitCommand
            {
                DateOfIssue = DateTime.Now,
                Description = "Unit Test",
                Value = 0
            };

            _createEntryDebitCommandHandler.Handle(command);

            _entryDebitRepository.Verify(r => r.Add(It.IsAny<Domain.Entities.EntryDebit>()), Times.Never);
        }
    }
}