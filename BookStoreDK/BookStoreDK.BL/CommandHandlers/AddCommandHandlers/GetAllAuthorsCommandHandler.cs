using System.Net;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.AuthorCommands;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.AddCommandHandlers
{
    public class GetAllAuthorsCommandHandler : IRequestHandler<GetAllAuthorsCommand, AuthorsCollectionResponse>
    {
        private readonly IAuthorRepository _authorRepository;
        

        public GetAllAuthorsCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
            
        }
        public async Task<AuthorsCollectionResponse> Handle(GetAllAuthorsCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorRepository.GetAll();

            return new AuthorsCollectionResponse()
            {
                Model = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
