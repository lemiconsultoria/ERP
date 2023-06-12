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
    public class DeleteEntryCreditCommandHandler : CommandHandlerBase<DeleteEntryCreditCommandResult>, ICommandHandler<DeleteEntryCreditCommand, DeleteEntryCreditCommandResult>
    {
        private readonly IValidator<DeleteEntryCreditCommand> _deleteCommandValidator;
        private readonly IEntryCreditRepository _entryCreditRepository;
        private readonly IEventPublisher<CreditEntryEvent> _eventPublisher;

        public DeleteEntryCreditCommandHandler(IValidator<DeleteEntryCreditCommand> validator, IEntryCreditRepository entryCreditRepository, IEventPublisher<CreditEntryEvent> eventPublisher)
        {
            _deleteCommandValidator = validator;
            _entryCreditRepository = entryCreditRepository;
            _eventPublisher = eventPublisher;
        }

        public CommandResult<DeleteEntryCreditCommandResult> Handle(DeleteEntryCreditCommand command)
        {
            var validationResult = Validate(command, _deleteCommandValidator);

            if (validationResult.IsValid)
            {
                var entity = _entryCreditRepository.GetByIdAsync(command.Id).Result;

                if (entity != null && entity.Id > 0)
                {
                    _entryCreditRepository.Delete(entity);
                    _entryCreditRepository.SaveChanges();

                    var eventMessage = Mapper.EntityToEvent<EntryCredit, CreditEntryEvent>(entity);
                    eventMessage.Operation = OperationType.Delete;

                    _eventPublisher.Publish(eventMessage);
                }

                return Return();
            }

            return Return();
        }
    }
}