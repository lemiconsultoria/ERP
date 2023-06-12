using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Core.AutoMapper;
using ERP.Domain.Events;
using Microsoft.Extensions.Configuration;

namespace ERP.Consolidator.Infra.Messaging
{
    public class DebitEntryEventConsumer : BaseEventConsumer<DebitEntryEvent, Guid>
    {
        private readonly IEntryDebitRepository _entryDebitRepository;

        public DebitEntryEventConsumer(IConfiguration configuration, IEntryDebitRepository entryDebitRepository, IDateToProcessRepository dateToProcessRepository) : base(configuration, dateToProcessRepository)
        {
            _entryDebitRepository = entryDebitRepository;

            _queueName = configuration.GetSection("RabbitMQ").GetSection("Queue").GetSection("Debit").Value ?? "";
            _queueNameRetry = $"{_queueName}_retry";
        }

        public override void HandleMessage(DebitEntryEvent message)
        {
            if (message == null)
            {
                throw new ArgumentNullException($"Message is null {typeof(DebitEntryEvent)}");
            }

            var entity = Mapper.EventToEntity<DebitEntryEvent, EntryDebit>(message);
            entity.DateRef = DateOnly.FromDateTime(message.DateOfIssue);

            bool modified = false;

            if (message.Operation == Core.Messages.OperationType.Insert)
            {
                _entryDebitRepository.Add(entity);
                modified = true;
            }
            else if (message.Operation == Core.Messages.OperationType.Update)
            {
                var entityBefore = _entryDebitRepository.GetById(entity.Id);

                if (entityBefore.Id == entity.Id)
                {
                    _entryDebitRepository.Update(entity);
                    modified = true;

                    if (entityBefore.DateRef != entity.DateRef)
                        RegisterDateToProcess(entityBefore.DateRef);
                }
            }
            else if (message.Operation == Core.Messages.OperationType.Delete)
            {
                _entryDebitRepository.Delete(entity);
                modified = true;
            }

            if (modified)
                RegisterDateToProcess(entity.DateRef);
        }
    }
}