using BookStoreDK.Models.Requests;
using FluentValidation;

namespace BookStoreDK.Validators
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(200);

            RuleFor(x => x.AuthorId)
                .GreaterThan(0);

            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    
    }
}
