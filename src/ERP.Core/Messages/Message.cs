namespace ERP.Core.Messages
{
    public abstract class Message : IMessage<Guid>
    {
        public Guid IdMessage { get; set; }

        public OperationType Operation { get; set; }

        public Message()
        {
            IdMessage = Guid.NewGuid();
            Operation = OperationType.NotApplicable;
        }
    }

    public enum OperationType
    {
        NotApplicable = 0, Insert = 1, Update = 2, Delete = 3,
    }
}