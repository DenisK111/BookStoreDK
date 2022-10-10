using System.Net;
using BookStoreDK.BL.Helpers;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.AuthorCommands;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.AddCommandHandlers
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, AuthorResponse>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository,IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }
        public async Task<AuthorResponse> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorId = request.authorId;
            var booksCount = await _bookRepository.GetBooksCountByAuthorId(authorId);
            if (booksCount > 0)
            {
                return new AuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Cannot Delete Author With Books"

                };
            }

            var result = await _authorRepository.Delete(authorId);
            return NullChecker.CheckForNullObjectAndReturnResponse<Author,AuthorResponse>(result, "Id does not exist");
        }
    }
}
