using System.Reflection;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.Extensions;
using BookStoreDK.Models.MediatR.Commands;
using BookStoreDK.Models.MediatR.Commands.BookCommands;
using BookStoreDK.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return this.ProduceResponse(await _mediator.Send(new GetAllBooksCommand()));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetById))]
        public async Task<IActionResult> GetById(int id)
        {
            return this.ProduceResponse(await _mediator.Send(new GetBookByIdCommand(id)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddBookRequest model)
        {
            return this.ProduceResponse(await _mediator.Send(new AddBookCommand(model)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookRequest model)
        {
            return this.ProduceResponse(await _mediator.Send(new UpdateBookCommand(model)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            return this.ProduceResponse(await _mediator.Send(new DeleteBookCommand(id)));
        }
    }
}
