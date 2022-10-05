using BookStoreDK.Models.Requests;
using FluentValidation;

namespace BookStoreDK.Validators
{
    public class AddAuthorRequestValidator : AbstractValidator<AddAuthorRequest>
    {
        public AddAuthorRequestValidator()
        {
            RuleFor(x => x.Age)
                .GreaterThan(0)
                .LessThanOrEqualTo(120);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.NickName)
               .MaximumLength(10)
               .MinimumLength(2)
               .When(x => !string.IsNullOrEmpty(x.NickName), ApplyConditionTo.CurrentValidator);

            RuleFor(x => x.DateOfBirth)
                .GreaterThan(DateTime.MinValue)
                .LessThan(DateTime.MaxValue);
        }
    }
}
