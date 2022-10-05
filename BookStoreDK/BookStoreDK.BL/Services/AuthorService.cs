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
        private readonly IBookRepository _booksRepo;

        public AuthorService(IAuthorRepository repo, IMapper mapper, IBookRepository booksRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _booksRepo = booksRepo;
        }

        public async Task<AuthorResponse> Add(AddAuthorRequest author)
        {
            {
                var auth = await _repo.GetAuthorByName(author.Name);

                if (auth != null)
                    return new AuthorResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Author already exists"
                    };
                var authorObject = _mapper.Map<Author>(author);
                var result = await _repo.Add(authorObject);

                return new AuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Author = result
                };
            }
        }

        public async Task<AuthorsCollectionResponse> AddRange(AddMultipleAuthorsRequest model)
        {
            var authorCollection = _mapper.Map<IEnumerable<Author>>(model.AuthorRequests);
            var result = await _repo.AddMultipleAuthors(authorCollection);

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
                Authors = authorCollection,
            };
        }

        public async Task<AuthorResponse> Delete(int modelId)
        {
            
            var booksCount = await _booksRepo.GetBooksCountByAuthorId(modelId);
            if (booksCount > 0)
            {
                return new AuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Cannot Delete Author With Books"

                };
            }

            var result = await _repo.Delete(modelId);
            return CheckForNullAndReturnResponse(result, "Id does not exist");

        }

        public async Task<AuthorsCollectionResponse> GetAll()
        {
            var result = await _repo.GetAll();

            return new AuthorsCollectionResponse()
            {
                Authors = result,
                HttpStatusCode = HttpStatusCode.OK
            };

        }


        public async Task<AuthorResponse> GetById(int id)
        {
            var result = await _repo.GetById(id);
            return CheckForNullAndReturnResponse(result,"Id does not exist");
                      
        }

        public async Task<AuthorResponse> Update(UpdateAuthorRequest model)
        {
            var modelToUpdate = await GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new AuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist"
                };
            }
            var authorObject = _mapper.Map<Author>(model);
            var result = await _repo.Update(authorObject);

            return new AuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Author = result,
            };

        }

        private AuthorResponse CheckForNullAndReturnResponse(Author? result,string errorMessage = "")
        {
            if (result == null)
            {
                return new AuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = errorMessage,
                };
            }

            return new AuthorResponse()
            {
                Author = result,
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
    }
}
