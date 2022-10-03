using BookStoreDK.BL.Interfaces;
using BookStoreDK.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {

        private readonly ILogger<BookController> _logger;
        private readonly IBookService _bookService;

        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public IEnumerable<Book> Get()
        {

            return _bookService.GetAll();

        }

        [HttpGet(nameof(GetById))]

        public Book? GetById(int Id)
        {
            return _bookService.GetById(Id);

        }

        [HttpPost]
        public Book? Add([FromBody] Book model)
        {
            return _bookService.Add(model);


        }

        [HttpPut]
        public Book? Update([FromBody] Book model)
        {
            return _bookService.Update(model);
        }

        [HttpDelete]
        public Book? Delete([FromBody] int id)
        {
            return _bookService.Delete(id);
        }
    }
}
