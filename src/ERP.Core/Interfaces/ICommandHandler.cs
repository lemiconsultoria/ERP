using ERP.Core.Commands;

namespace ERP.Core.Interfaces
{
    public interface ICommandHandler<in T, TDataResult> where T : CommandBase where TDataResult : CommandDataResult
    {
        CommandResult<TDataResult> Handle(T command);
    }
}