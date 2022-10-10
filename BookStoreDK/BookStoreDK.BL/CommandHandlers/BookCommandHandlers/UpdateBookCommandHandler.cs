using System.Net;
using AutoMapper;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.BookCommands;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.BookCommandHandlers
{
    internal class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler(IBookRepository bookRepository,IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        public async Task<BookResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var model = request.UpdateBookRequest;

            var modelToUpdate = await _bookRepository.GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Book does not exist"
                };
            }
            var bookObject = _mapper.Map<Book>(model);
            var result = await _bookRepository.Update(bookObject);

            return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = result,
            };
        }
    }
}
