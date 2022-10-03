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

        public AddBookResponse Add(AddBookRequest model)
        {
            var auth = _repo.GetBookByTitle(model.Title);

            if (auth != null)
                return new AddBookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author already exist"
                };
            var bookObject = _mapper.Map<Book>(model);
            var result = _repo.Add(bookObject);

            return new AddBookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = result
            };
        }

        public Book? Delete(int modelId)
        {
            return _repo.Delete(modelId);
        }

        public IEnumerable<Book> GetAll()
        {
            return _repo.GetAll();
        }

        public Book? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public UpdateBookResponse Update(UpdateBookRequest model)
        {
            var modelToUpdate = GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new UpdateBookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist"
                };
            }
            var bookObject = _mapper.Map<Book>(model);
            var result = _repo.Update(bookObject);

            return new UpdateBookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = result,
            };
        }

        public Book? GetBookByName(string name)
        {
            return _repo.GetBookByTitle(name);
        }
    }
}
