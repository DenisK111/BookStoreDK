using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDK.BL.Helpers;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.BookCommands;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.BookCommandHandlers
{
    internal class GetBookByIdCommandHandler : IRequestHandler<GetBookByIdCommand, BookResponse>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookByIdCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookResponse> Handle(GetBookByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _bookRepository.GetById(request.bookId);
            return NullChecker.CheckForNullObjectAndReturnResponse<Book,BookResponse>(result, "Id does not exist");
        }
    }
}
