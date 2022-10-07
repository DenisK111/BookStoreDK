using System.Net;
using System.Reflection;
using AutoMapper;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.BookCommands;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.BookCommandHandlers
{
    internal class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AddBookCommandHandler(IBookRepository bookRepository,IAuthorRepository authorRepository,IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public async Task<BookResponse> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var model = request.AddBookRequest;

            var book = await _bookRepository.GetBookByTitle(model.Title);

            if (book != null)
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Book already exists"
                };

            var author = await _authorRepository.GetById(model.AuthorId);

            if (author == null)
            {
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author Id Does not Exist"
                };
            }

            var bookObject = _mapper.Map<Book>(model);
            var result = await _bookRepository.Add(bookObject);

            return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = result
            };
        }
    }
}
