
using BookStoreDK.Extensions;
using BookStoreDK.KafkaCache;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.MediatR.Commands.BookCommands;
using BookStoreDK.Models.Models;
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
       
        private readonly KafkaConsumer<int, Book,KafkaBookConsumerSettings> _kafkaConsumer;
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public BookController(IMediator mediator, KafkaConsumer<int, Book, KafkaBookConsumerSettings> kafkaConsumer)
        {
            _mediator = mediator;
           
            _kafkaConsumer = kafkaConsumer;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_kafkaConsumer.ConsumerDictionary);
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

        [HttpPost(nameof(AddIndex))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddIndex([FromBody] string propertyName)
        {
            var result =  _kafkaConsumer.AddIndex(propertyName);

            if (!result.isSuccess)
            {
                return BadRequest(result.message);
            }

            return Ok();
        }
    }
}
