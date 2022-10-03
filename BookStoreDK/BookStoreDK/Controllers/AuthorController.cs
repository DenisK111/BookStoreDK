using System.Net;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly IAuthorService _authorService;

        public AuthorController(ILogger<AuthorController> logger, IAuthorService authorService)
        {
            _logger = logger;
            _authorService = authorService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(Get))]
        public IActionResult Get()
        {
            _logger.LogInformation("Test");
            _logger.LogDebug("Test");
            return Ok(_authorService.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(nameof(GetById))]
        public IActionResult GetById(int Id)
        {
            var result = _authorService.GetById(Id);

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
        public IActionResult Add([FromBody] AddAuthorRequest request)
        {
            AddAuthorResponse result = _authorService.Add(request);
            try
            {
                if (result!.HttpStatusCode == HttpStatusCode.BadRequest)
                {
                    throw new ArgumentException("Author already exists");
                }

                return Ok(result);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new
                {
                    error=ex.Message
                });
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public IActionResult Update([FromBody] UpdateAuthorRequest model)
        {
            var result = _authorService.Update(model);

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
            var result = _authorService.Delete(id);

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
