namespace ERP.Core.Messages
{
    public interface IMessage<TPrimaryKey>
    {
        TPrimaryKey IdMessage { get; set; }
    }
}