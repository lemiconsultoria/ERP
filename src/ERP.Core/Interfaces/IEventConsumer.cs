using ERP.Core.Messages;

namespace ERP.Core.Interfaces
{
    public interface IEventConsumer<TMessage, TPrimaryKey>
        where TMessage : class, IMessage<TPrimaryKey>
    {
        void Subscribe();

        void HandleMessage(TMessage message);
    }
}