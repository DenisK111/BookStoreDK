using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.Models.MediatR.Commands.BookCommands
{
    public record GetAllBooksCommand : IRequest<BookCollectionResponse>
    {
    }
}
