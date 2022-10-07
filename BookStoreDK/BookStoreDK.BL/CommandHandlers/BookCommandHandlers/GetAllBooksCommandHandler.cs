using System.Net;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.BookCommands;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.BookCommandHandlers
{
    public class GetAllBooksCommandHandler : IRequestHandler<GetAllBooksCommand, BookCollectionResponse>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookCollectionResponse> Handle(GetAllBooksCommand request, CancellationToken cancellationToken)
        {
            var result = await _bookRepository.GetAll();

            return new BookCollectionResponse()
            {
                Model = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
