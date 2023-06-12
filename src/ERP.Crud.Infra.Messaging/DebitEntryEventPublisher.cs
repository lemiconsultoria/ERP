using ERP.Core.Messages;
using Microsoft.Extensions.Configuration;

namespace ERP.Crud.Infra.Messaging
{
    public class DebitEntryEventPublisher<TMessage> : BaseEventPublisher<TMessage> where TMessage : Message
    {
        public DebitEntryEventPublisher(IConfiguration configuration) : base(configuration)
        {
            _queueName = configuration.GetSection("RabbitMQ")?.GetSection("Queue")?.GetSection("Debit").Value ?? "";
            _queueNameRetry = $"{_queueName}_retry";
        }
    }
}