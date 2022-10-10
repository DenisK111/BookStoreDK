using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.Models.MediatR.Commands.AuthorCommands
{
    public record DeleteAuthorCommand(int authorId) : IRequest<AuthorResponse>
    {
    }
}
