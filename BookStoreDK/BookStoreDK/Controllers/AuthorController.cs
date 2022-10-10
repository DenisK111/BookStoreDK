using BookStoreDK.Extensions;
using BookStoreDK.Models.MediatR.Commands.AuthorCommands;
using BookStoreDK.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {        
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {            
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(Get))]
        public async Task<IActionResult> Get()
        {
            return this.ProduceResponse(await _mediator.Send(new GetAllAuthorsCommand()));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetById))]
        public async Task<IActionResult> GetById(int id)
        {
            return this.ProduceResponse(await _mediator.Send(new GetAuthorByIdCommand(id)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddAuthorRequest request)
        {
            
            return this.ProduceResponse(await _mediator.Send(new AddAuthorCommand(request)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(AddAuthorRange))]
        public async Task<IActionResult> AddAuthorRange([FromBody] AddMultipleAuthorsRequest addMultipleAuthorRequests)
        {
            return this.ProduceResponse(await _mediator.Send(new AddAuthorRangeCommand(addMultipleAuthorRequests)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAuthorRequest model)
        {
            return this.ProduceResponse(await _mediator.Send(new UpdateAuthorCommand(model)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            return this.ProduceResponse(await _mediator.Send(new DeleteAuthorCommand(id)));
        }
    }
}
