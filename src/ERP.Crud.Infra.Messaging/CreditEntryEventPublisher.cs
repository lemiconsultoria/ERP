using ERP.Core.Messages;
using Microsoft.Extensions.Configuration;

namespace ERP.Crud.Infra.Messaging
{
    public class CreditEntryEventPublisher<TMessage> : BaseEventPublisher<TMessage> where TMessage : Message
    {
        public CreditEntryEventPublisher(IConfiguration configuration) : base(configuration)
        {
            _queueName = configuration.GetSection("RabbitMQ")?.GetSection("Queue")?.GetSection("Credit").Value ?? "";
            _queueNameRetry = $"{_queueName}_retry";
        }
    }
}