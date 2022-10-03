using System.Net;
using AutoMapper;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repo;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public AddAuthorResponse Add(AddAuthorRequest author)
        {
            {
                var auth = _repo.GetAuthorByName(author.Name);

                if (auth != null)
                    return new AddAuthorResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Author already exist"
                    };
                var authorObject = _mapper.Map<Author>(author);
                var result = _repo.Add(authorObject);

                return new AddAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Author = result
                };
            }
        }

        public AddAuthorsResponse AddRange(AddMultipleAuthorsRequest model)
        {
            var authorCollection = _mapper.Map<IEnumerable<Author>>(model.AuthorRequests);
            var result = _repo.AddMultipleAuthors(authorCollection);

            if (!result)
            {
                return new AddAuthorsResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to add authors"
                };
            }

            return new AddAuthorsResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Persons = authorCollection,
            };
        }

        public Author? Delete(int modelId)
        {
            return _repo.Delete(modelId);
        }

        public IEnumerable<Author> GetAll()
        {
            return _repo.GetAll();
        }


        public Author? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public UpdateAuthorResponse Update(UpdateAuthorRequest model)
        {
            var modelToUpdate = GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new UpdateAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist"
                };
            }
            var authorObject = _mapper.Map<Author>(model);
            var result = _repo.Update(authorObject);

            return new UpdateAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Author = result,
            };

        }
    }
}
