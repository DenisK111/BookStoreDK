using System.Net;
using AutoMapper;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepo;

        public BookService(IBookRepository repo, IMapper mapper,IAuthorRepository authorRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _authorRepo = authorRepo;
        }

        public async Task<BookResponse> Add(AddBookRequest model)
        {
            var book = await _repo.GetBookByTitle(model.Title);

            if (book != null)
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Book already exists"
                };

            var author = await _authorRepo.GetById(model.AuthorId);

            if (author == null)
            {
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author Id Does not Exist"
                };
            }

            var bookObject = _mapper.Map<Book>(model);
            var result = await _repo.Add(bookObject);

            return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = result
            };
        }

        public async Task<BookResponse> Delete(int modelId)
        {
            var result = await _repo.Delete(modelId);
            return CheckForNullAndReturnResponse(result, "Id does not exist");

        }

        public async Task<BookCollectionResponse> GetAll()
        {

            var result = await _repo.GetAll();

            return new BookCollectionResponse()
            {
                Books = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public async Task<BookResponse> GetById(int id)
        {
            var result = await _repo.GetById(id);
            return CheckForNullAndReturnResponse(result, "Id does not exist");
        }

        public async Task<BookResponse> Update(UpdateBookRequest model)
        {
            var modelToUpdate = GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist"
                };
            }
            var bookObject = _mapper.Map<Book>(model);
            var result = await _repo.Update(bookObject);

            return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = result,
            };
        }


        private BookResponse CheckForNullAndReturnResponse(Book? result, string errorMessage = "")
        {
            if (result == null)
            {
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = errorMessage,
                };
            }

            return new BookResponse()
            {
                Book = result,
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
    }
}
