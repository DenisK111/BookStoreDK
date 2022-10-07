using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.Models.MediatR.Commands.AuthorCommands
{
    public record GetAuthorByIdCommand(int authorId) : IRequest<AuthorResponse>
    {
    }
}
