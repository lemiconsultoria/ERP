using ERP.Core.Messages;

namespace ERP.Core.Interfaces
{
    public interface IEventPublisher<in TMessage> where TMessage : Message
    {
        bool Publish(TMessage message);
    }
}