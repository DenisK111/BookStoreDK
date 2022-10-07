using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.Models.MediatR.Commands.AuthorCommands
{
    public record UpdateAuthorCommand(UpdateAuthorRequest UpdateAuthorRequest) : IRequest<AuthorResponse>
    {
    }
}
