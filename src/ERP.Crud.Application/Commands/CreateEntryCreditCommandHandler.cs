using ERP.Core.AutoMapper;
using ERP.Core.Commands;
using ERP.Core.Interfaces;
using ERP.Core.Messages;
using ERP.Crud.Domain.Commands.EntryCredit;
using ERP.Crud.Domain.Commands.EntryCredit.Results;
using ERP.Crud.Domain.Entities;
using ERP.Crud.Domain.Interfaces.Repositories;
using ERP.Domain.Events;
using FluentValidation;

namespace ERP.Crud.Application.Commands
{
    public class CreateEntryCreditCommandHandler : CommandHandlerBase<EntryCreditCommandResultBase>, ICommandHandler<CreateEntryCreditCommand, EntryCreditCommandResultBase>
    {
        private readonly IValidator<CreateEntryCreditCommand> _createCommandValidator;
        private readonly IEntryCreditRepository _entryCreditRepository;
        private readonly IEventPublisher<CreditEntryEvent> _eventPublisher;

        public CreateEntryCreditCommandHandler(IValidator<CreateEntryCreditCommand> validator, IEntryCreditRepository entryCreditRepository, IEventPublisher<CreditEntryEvent> eventPublisher)
        {
            _createCommandValidator = validator;
            _entryCreditRepository = entryCreditRepository;
            _eventPublisher = eventPublisher;
        }

        public CommandResult<EntryCreditCommandResultBase> Handle(CreateEntryCreditCommand command)
        {
            var validationResult = Validate(command, _createCommandValidator);

            if (validationResult.IsValid)
            {
                var entry = Mapper.CommandToEntity<CreateEntryCreditCommand, EntryCredit>(command);
                _entryCreditRepository.Add(entry);
                _entryCreditRepository.SaveChanges();

                var eventMessage = Mapper.EntityToEvent<EntryCredit, CreditEntryEvent>(entry);
                eventMessage.Operation = OperationType.Insert;

                _eventPublisher.Publish(eventMessage);

                var result = Mapper.EntityToCommandResult<EntryCredit, EntryCreditCommandResultBase>(entry);

                return Return(result);
            }

            return Return();
        }
    }
}