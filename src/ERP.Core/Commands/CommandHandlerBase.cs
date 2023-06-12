using FluentValidation;
using FluentValidation.Results;

namespace ERP.Core.Commands
{
    public abstract class CommandHandlerBase<TDataResult> where TDataResult : CommandDataResult
    {
        protected IEnumerable<string>? Notifications;

        protected ValidationResult Validate<T, TValidator>(
            T command,
            TValidator validator)
            where T : CommandBase
            where TValidator : IValidator<T>
        {
            var validationResult = validator.Validate(command);
            Notifications = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

            return validationResult;
        }

        public CommandResult<TDataResult> ReturnError(string error)
        {
            var notification = new List<string>
            {
                error
            };

            return new(false, null, notification);
        }

        public CommandResult<TDataResult> Return()
        {
            return Return(null);
        }

        public CommandResult<TDataResult> Return(TDataResult? data)
        {
            Notifications ??= new List<string>();
            return new(!Notifications.Any(), data, Notifications);
        }
    }
}