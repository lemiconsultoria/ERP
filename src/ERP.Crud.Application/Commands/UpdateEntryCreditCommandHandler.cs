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
    public class UpdateEntryCreditCommandHandler : CommandHandlerBase<EntryCreditCommandResultBase>, ICommandHandler<UpdateEntryCreditCommand, EntryCreditCommandResultBase>
    {
        private readonly IValidator<UpdateEntryCreditCommand> _updateCommandValidator;
        private readonly IEntryCreditRepository _entryCreditRepository;
        private readonly IEventPublisher<CreditEntryEvent> _eventPublisher;

        public UpdateEntryCreditCommandHandler(IValidator<UpdateEntryCreditCommand> validator, IEntryCreditRepository entryCreditRepository, IEventPublisher<CreditEntryEvent> eventPublisher)
        {
            _updateCommandValidator = validator;
            _entryCreditRepository = entryCreditRepository;
            _eventPublisher = eventPublisher;
        }

        public CommandResult<EntryCreditCommandResultBase> Handle(UpdateEntryCreditCommand command)
        {
            var validationResult = Validate(command, _updateCommandValidator);

            if (validationResult.IsValid)
            {
                var entity = _entryCreditRepository.GetByIdAsync(command.Id).Result;

                if (entity != null && entity.Id > 0)
                {
                    var entry = Mapper.CommandToEntity<UpdateEntryCreditCommand, EntryCredit>(command);
                    _entryCreditRepository.Update(entry);
                    _entryCreditRepository.SaveChanges();

                    var eventMessage = Mapper.EntityToEvent<EntryCredit, CreditEntryEvent>(entry);
                    eventMessage.Operation = OperationType.Update;

                    _eventPublisher.Publish(eventMessage);
                }
                return Return();
            }

            return Return();
        }
    }
}