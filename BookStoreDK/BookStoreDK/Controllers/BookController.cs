using BookStoreDK.BL.Kafka;
using BookStoreDK.Extensions;
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
        private readonly KafkaProducer<int, Book> _kafkaBookProducer;
        private readonly KafkaConsumer<int, Book> _kafkaConsumer;
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public BookController(IMediator mediator, KafkaProducer<int, Book> kafkaBookProducer, KafkaConsumer<int, Book> kafkaConsumer)
        {
            _mediator = mediator;
            _kafkaBookProducer = kafkaBookProducer;
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
            var id = _random.Next(0, int.MaxValue);

            var message = new Book()
            {
                Title = model.Title,
                Quantity = model.Quantity,
                LastUpdated = DateTime.UtcNow,
                Price = model.Price,
                AuthorId = model.AuthorId,
                Id = id
            };

            var result = await _kafkaBookProducer.Produce(id, message);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result.Value);
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
