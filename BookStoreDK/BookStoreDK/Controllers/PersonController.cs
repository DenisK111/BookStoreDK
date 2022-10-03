using System.Net;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(Get))]
        public IActionResult Get()
        {
            return Ok(_personService.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(nameof(GetById))]
        public IActionResult GetById(int Id)
        {
            var result = _personService.GetById(Id);

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
        public IActionResult Add([FromBody] AddPersonRequest request)
        {
            var result = _personService.Add(request);

            if (result!.HttpStatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public IActionResult Update([FromBody] UpdatePersonRequest model)
        {
            var result = _personService.Update(model);

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
            var result = _personService.Delete(id);

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