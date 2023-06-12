namespace ERP.Core.Commands
{
    public class CommandResult<T> where T : CommandDataResult
    {
        public IEnumerable<string> Errors { get; }
        public bool Success { get; }
        public T? Data { get; }

        public CommandResult(bool success, T? data, IEnumerable<string> errors)
        {
            Success = success;
            Errors = errors;
            Data = data;
        }
    }
}