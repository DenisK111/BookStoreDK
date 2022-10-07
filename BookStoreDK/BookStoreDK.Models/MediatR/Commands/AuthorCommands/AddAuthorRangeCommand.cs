using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.Models.MediatR.Commands.AuthorCommands
{
    public record AddAuthorRangeCommand(AddMultipleAuthorsRequest AddMultipleAuthorsRequest) : IRequest<AuthorsCollectionResponse>
    {
    }
}
