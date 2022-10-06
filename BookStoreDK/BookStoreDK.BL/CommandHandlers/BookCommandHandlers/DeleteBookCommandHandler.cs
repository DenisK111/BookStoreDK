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
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, BookResponse>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<BookResponse> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var result = await _bookRepository.Delete(request.bookId);
            return NullChecker.CheckForNullObjectAndReturnResponse<Book,BookResponse>(result, "Id does not exist");
        }
    }
}
