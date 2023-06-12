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
    public class DeleteEntryDebitCommandHandler : CommandHandlerBase<DeleteEntryDebitCommandResult>, ICommandHandler<DeleteEntryDebitCommand, DeleteEntryDebitCommandResult>
    {
        private readonly IValidator<DeleteEntryDebitCommand> _deleteCommandValidator;
        private readonly IEntryDebitRepository _entryDebitRepository;
        private readonly IEventPublisher<DebitEntryEvent> _eventPublisher;

        public DeleteEntryDebitCommandHandler(IValidator<DeleteEntryDebitCommand> validator, IEntryDebitRepository entryDebitRepository, IEventPublisher<DebitEntryEvent> eventPublisher)
        {
            _deleteCommandValidator = validator;
            _entryDebitRepository = entryDebitRepository;
            _eventPublisher = eventPublisher;
        }

        public CommandResult<DeleteEntryDebitCommandResult> Handle(DeleteEntryDebitCommand command)
        {
            var validationResult = Validate(command, _deleteCommandValidator);

            if (validationResult.IsValid)
            {
                var entity = _entryDebitRepository.GetByIdAsync(command.Id).Result;

                if (entity != null && entity.Id > 0)
                {
                    _entryDebitRepository.Delete(entity);
                    _entryDebitRepository.SaveChanges();

                    var eventMessage = Mapper.EntityToEvent<EntryDebit, DebitEntryEvent>(entity);
                    eventMessage.Operation = OperationType.Delete;

                    _eventPublisher.Publish(eventMessage);
                }

                return Return();
            }

            return Return();
        }
    }
}