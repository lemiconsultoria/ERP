using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Core.AutoMapper;
using ERP.Domain.Events;
using Microsoft.Extensions.Configuration;

namespace ERP.Consolidator.Infra.Messaging
{
    public class CreditEntryEventConsumer : BaseEventConsumer<CreditEntryEvent, Guid>
    {
        private readonly IEntryCreditRepository _entryCreditRepository;

        public CreditEntryEventConsumer(IConfiguration configuration, IEntryCreditRepository entryCreditRepository, IDateToProcessRepository dateToProcessRepository) : base(configuration, dateToProcessRepository)
        {
            _entryCreditRepository = entryCreditRepository;

            _queueName = configuration.GetSection("RabbitMQ").GetSection("Queue").GetSection("Credit").Value ?? "";
            _queueNameRetry = $"{_queueName}_retry";
        }

        public override void HandleMessage(CreditEntryEvent message)
        {
            if (message == null)
            {
                throw new ArgumentNullException($"Message is null {typeof(DebitEntryEvent)}");
            }

            var entity = Mapper.EventToEntity<CreditEntryEvent, EntryCredit>(message);
            entity.DateRef = DateOnly.FromDateTime(message.DateOfIssue);

            bool modified = false;

            if (message.Operation == Core.Messages.OperationType.Insert)
            {
                _entryCreditRepository.Add(entity);
                modified = true;
            }
            else if (message.Operation == Core.Messages.OperationType.Update)
            {
                var entityBefore = _entryCreditRepository.GetById(entity.Id);

                if (entityBefore.Id == entity.Id)
                {
                    _entryCreditRepository.Update(entity);
                    modified = true;

                    if (entityBefore.DateRef != entity.DateRef)
                        RegisterDateToProcess(entityBefore.DateRef);
                }
            }
            else if (message.Operation == Core.Messages.OperationType.Delete)
            {
                _entryCreditRepository.Delete(entity);
                modified = true;
            }

            if (modified)
                RegisterDateToProcess(entity.DateRef);
        }
    }
}