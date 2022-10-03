using System.Net;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.Models.Requests;
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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_bookService.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(nameof(GetById))]
        public IActionResult GetById(int Id)
        {
            var result = _bookService.GetById(Id);

            if (result == null)
            {
                return BadRequest(new
                {
                    error = "Id does not exist"
                });
            }

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult Add([FromBody] AddBookRequest model)
        {
            var result = _bookService.Add(model);

            if (result!.HttpStatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }

            return Ok(result);


        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public IActionResult Update([FromBody] UpdateBookRequest model)
        {
            var result = _bookService.Update(model);

            if (result!.HttpStatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            var result = _bookService.Delete(id);

            if (result == null)
            {
                return BadRequest(new
                {
                    error = "Id does not exist"
                });
            }

            return Ok(result);

        }
    }
}
