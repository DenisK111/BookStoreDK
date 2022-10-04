using System.Net;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.Extensions;
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
        public async Task<IActionResult> Get()
        {
            return Ok(await _bookService.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(nameof(GetById))]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _bookService.GetById(Id);

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
        public async Task<IActionResult> Add([FromBody] AddBookRequest model)
        {
            return this.ProduceResponse(await _bookService.Add(model));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookRequest model)
        {
            return this.ProduceResponse(await _bookService.Update(model));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var result = await _bookService.Delete(id);

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
