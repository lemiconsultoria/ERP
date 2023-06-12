using FluentValidation;

namespace ERP.Crud.Domain.Commands.EntryDebit.Validators
{
    public class UpdateEntryDebitCommandValidator : EntryDebitCommandValidatorBase<UpdateEntryDebitCommand>
    {
        public UpdateEntryDebitCommandValidator()
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