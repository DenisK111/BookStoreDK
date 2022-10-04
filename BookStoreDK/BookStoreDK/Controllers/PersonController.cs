using System.Net;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.Extensions;
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
        public async Task<IActionResult> Get()
        {
            return Ok(await _personService.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(nameof(GetById))]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _personService.GetById(Id);

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
        public async Task<IActionResult> Add([FromBody] AddPersonRequest request)
        {
            return this.ProduceResponse(await _personService.Add(request));
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePersonRequest model)
        {
            return this.ProduceResponse(await _personService.Update(model));
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var result = await _personService.Delete(id);

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