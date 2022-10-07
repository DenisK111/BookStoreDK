using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.Models.MediatR.Commands.BookCommands
{
    public record UpdateBookCommand(UpdateBookRequest UpdateBookRequest) : IRequest<BookResponse>
    {
    }
}
