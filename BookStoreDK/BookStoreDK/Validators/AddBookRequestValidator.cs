using BookStoreDK.Models.Requests;
using FluentValidation;

namespace BookStoreDK.Validators
{
    public class AddBookRequestValidator : AbstractValidator<AddBookRequest>
    {

        public AddBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(200);

            RuleFor(x => x.AuthorId)
                .GreaterThan(0);
        }

    }
}
