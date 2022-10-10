using System.Net;
using AutoMapper;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.AuthorCommands;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.AddCommandHandlers
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, AuthorResponse>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public async Task<AuthorResponse> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = request.AddAuthorRequest;
            var auth = await _authorRepository.GetAuthorByName(author.Name);

            if (auth != null)
                return new AuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author already exists"
                };
            var authorObject = _mapper.Map<Author>(author);
            var result = await _authorRepository.Add(authorObject);

            return new AuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = result
            };
        }
    }
}
