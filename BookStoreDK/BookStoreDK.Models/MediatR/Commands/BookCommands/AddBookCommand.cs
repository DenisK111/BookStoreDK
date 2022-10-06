using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.Models.MediatR.Commands.BookCommands
{
    public record AddBookCommand(AddBookRequest AddBookRequest) : IRequest<BookResponse>
    {
    }
}
