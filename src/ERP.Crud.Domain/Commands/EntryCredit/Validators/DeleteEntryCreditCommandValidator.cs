using FluentValidation;

namespace ERP.Crud.Domain.Commands.EntryCredit.Validators
{
    public class DeleteEntryCreditCommandValidator : AbstractValidator<DeleteEntryCreditCommand>
    {
        public DeleteEntryCreditCommandValidator()
        {
            ValidateId();
        }

        private void ValidateId()
        {
            RuleFor(obj => obj.Id)
                .Must(id => id > 0)
                .WithSeverity(Severity.Error)
                .WithMessage("Id can't be empty");
        }
    }
}