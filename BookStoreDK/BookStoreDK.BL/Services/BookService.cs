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

        public BookService(IBookRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BookResponse> Add(AddBookRequest model)
        {
            var auth = await _repo.GetBookByTitle(model.Title);

            if (auth != null)
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author already exist"
                };
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
            // return await _repo.Delete(modelId);
            return null;
        }

        public async Task<BookResponseCollectionResponse> GetAll()
        {
            // return await _repo.GetAll();
            return null;
        }

        public async Task<BookResponse> GetById(int id)
        {
            //  return await _repo.GetById(id);
            return null;
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

        public async Task<BookResponse> GetBookByName(string name)
        {
          //  return await _repo.GetBookByTitle(name);
            return null;
        }
    }
}
