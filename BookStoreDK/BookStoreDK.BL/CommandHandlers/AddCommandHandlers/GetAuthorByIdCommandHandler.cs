using BookStoreDK.BL.Helpers;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.AuthorCommands;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.AddCommandHandlers
{
    public class GetAuthorByIdCommandHandler : IRequestHandler<GetAuthorByIdCommand, AuthorResponse>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAuthorByIdCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;

        }
        public async Task<AuthorResponse> Handle(GetAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorRepository.GetById(request.authorId);
            return NullChecker.CheckForNullObjectAndReturnResponse<Author, AuthorResponse>(result, "Id does not exist");
        }
    }
}
