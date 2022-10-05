using BookStoreDK.Models.Requests;
using FluentValidation;

namespace BookStoreDK.Validators
{
    public class UpdatePersonRequestValidator : AbstractValidator<UpdatePersonRequest>
    {
        public UpdatePersonRequestValidator()
        {
            RuleFor(x => x.Age)
             .GreaterThan(0)
             .LessThanOrEqualTo(120);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);


            RuleFor(x => x.DateOfBirth)
                .GreaterThan(DateTime.MinValue)
                .LessThan(DateTime.MaxValue);

            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
