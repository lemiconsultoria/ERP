using FluentValidation;

namespace ERP.Crud.Domain.Commands.EntryCredit.Validators
{
    public class UpdateEntryCreditCommandValidator : EntryCreditCommandValidatorBase<UpdateEntryCreditCommand>
    {
        public UpdateEntryCreditCommandValidator()
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