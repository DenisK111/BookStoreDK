using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.MediatR.Commands.AuthorCommands;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.BL.CommandHandlers.AddCommandHandlers
{
    internal class AddAuthorRangeCommandHandler : IRequestHandler<AddAuthorRangeCommand, AuthorsCollectionResponse>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AddAuthorRangeCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public async Task<AuthorsCollectionResponse> Handle(AddAuthorRangeCommand request, CancellationToken cancellationToken)
        {
            var model = request.AddMultipleAuthorsRequest;
            var authorCollection = _mapper.Map<IEnumerable<Author>>(model.AuthorRequests);
            var result = await _authorRepository.AddMultipleAuthors(authorCollection);

            if (!result)
            {
                return new AuthorsCollectionResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to add authors"
                };
            }

            return new AuthorsCollectionResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = authorCollection,
            };
        }
    }
}
