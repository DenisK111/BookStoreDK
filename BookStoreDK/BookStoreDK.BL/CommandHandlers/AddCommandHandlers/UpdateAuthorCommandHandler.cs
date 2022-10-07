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
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorResponse>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorResponse> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var model = request.UpdateAuthorRequest;
            var modelToUpdate = await _authorRepository.GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new AuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist"
                };
            }
            var authorObject = _mapper.Map<Author>(model);
            var result = await _authorRepository.Update(authorObject);

            return new AuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = result,
            };
        }
    }
}
