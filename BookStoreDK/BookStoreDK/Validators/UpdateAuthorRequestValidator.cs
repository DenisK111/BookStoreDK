using BookStoreDK.Models.Requests;
using FluentValidation;

namespace BookStoreDK.Validators
{
    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
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

            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
