using FluentValidation;

namespace ERP.Crud.Domain.Commands.EntryDebit.Validators
{
    public abstract class EntryDebitCommandValidatorBase<T> : AbstractValidator<T> where T : EntryDebitCommandBase
    {
        protected EntryDebitCommandValidatorBase()
        {
            ValidateDescription();
            ValidateValue();
        }

        private void ValidateDescription()
        {
            RuleFor(obj => obj.Description)
                .Must(description => !string.IsNullOrWhiteSpace(description))
                .WithSeverity(Severity.Error)
                .WithMessage("Description can't be empty");
        }

        private void ValidateValue()
        {
            RuleFor(obj => obj.Value)
                .Must(value => value > 0)
                .WithSeverity(Severity.Error)
                .WithMessage("Value can't be less than or equal to 0");
        }
    }
}