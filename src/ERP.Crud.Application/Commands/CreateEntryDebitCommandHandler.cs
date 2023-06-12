using ERP.Core.AutoMapper;
using ERP.Core.Commands;
using ERP.Core.Interfaces;
using ERP.Core.Messages;
using ERP.Crud.Domain.Commands.EntryDebit;
using ERP.Crud.Domain.Commands.EntryDebit.Results;
using ERP.Crud.Domain.Entities;
using ERP.Crud.Domain.Interfaces.Repositories;
using ERP.Domain.Events;
using FluentValidation;

namespace ERP.Crud.Application.Commands
{
    public class CreateEntryDebitCommandHandler : CommandHandlerBase<EntryDebitCommandResultBase>, ICommandHandler<CreateEntryDebitCommand, EntryDebitCommandResultBase>
    {
        private readonly IValidator<CreateEntryDebitCommand> _createCommandValidator;
        private readonly IEntryDebitRepository _entryDebitRepository;
        private readonly IEventPublisher<DebitEntryEvent> _eventPublisher;

        public CreateEntryDebitCommandHandler(IValidator<CreateEntryDebitCommand> validator, IEntryDebitRepository entryDebitRepository, IEventPublisher<DebitEntryEvent> eventPublisher)
        {
            _createCommandValidator = validator;
            _entryDebitRepository = entryDebitRepository;
            _eventPublisher = eventPublisher;
        }

        public CommandResult<EntryDebitCommandResultBase> Handle(CreateEntryDebitCommand command)
        {
            var validationResult = Validate(command, _createCommandValidator);

            if (validationResult.IsValid)
            {
                var entry = Mapper.CommandToEntity<CreateEntryDebitCommand, EntryDebit>(command);
                _entryDebitRepository.Add(entry);
                _entryDebitRepository.SaveChanges();

                var eventMessage = Mapper.EntityToEvent<EntryDebit, DebitEntryEvent>(entry);
                eventMessage.Operation = OperationType.Insert;

                _eventPublisher.Publish(eventMessage);

                var result = Mapper.EntityToCommandResult<EntryDebit, EntryDebitCommandResultBase>(entry);

                return Return(result);
            }

            return Return();
        }
    }
}