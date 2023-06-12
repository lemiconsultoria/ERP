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
    public class UpdateEntryDebitCommandHandler : CommandHandlerBase<EntryDebitCommandResultBase>, ICommandHandler<UpdateEntryDebitCommand, EntryDebitCommandResultBase>
    {
        private readonly IValidator<UpdateEntryDebitCommand> _updateCommandValidator;
        private readonly IEntryDebitRepository _entryDebitRepository;
        private readonly IEventPublisher<DebitEntryEvent> _eventPublisher;

        public UpdateEntryDebitCommandHandler(IValidator<UpdateEntryDebitCommand> validator, IEntryDebitRepository entryDebitRepository, IEventPublisher<DebitEntryEvent> eventPublisher)
        {
            _updateCommandValidator = validator;
            _entryDebitRepository = entryDebitRepository;
            _eventPublisher = eventPublisher;
        }

        public CommandResult<EntryDebitCommandResultBase> Handle(UpdateEntryDebitCommand command)
        {
            var validationResult = Validate(command, _updateCommandValidator);

            if (validationResult.IsValid)
            {
                var entity = _entryDebitRepository.GetByIdAsync(command.Id).Result;

                if (entity != null && entity.Id > 0)
                {
                    var entry = Mapper.CommandToEntity<UpdateEntryDebitCommand, EntryDebit>(command);
                    _entryDebitRepository.Update(entry);
                    _entryDebitRepository.SaveChanges();

                    var eventMessage = Mapper.EntityToEvent<EntryDebit, DebitEntryEvent>(entry);
                    eventMessage.Operation = OperationType.Update;

                    _eventPublisher.Publish(eventMessage);
                }
                return Return();
            }

            return Return();
        }
    }
}