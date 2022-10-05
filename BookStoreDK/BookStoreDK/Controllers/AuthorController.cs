using System.Net;
using System.Reflection;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.Extensions;
using BookStoreDK.Models.Models;
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
        public async Task<IActionResult> Get()
        {
            return this.ProduceResponse(await _authorService.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(nameof(GetById))]
        public async Task<IActionResult> GetById(int Id)
        {
            return this.ProduceResponse(await _authorService.GetById(Id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddAuthorRequest request)
        {
            return this.ProduceResponse(await _authorService.Add(request));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(AddAuthorRange))]
        public async Task<IActionResult> AddAuthorRange([FromBody] AddMultipleAuthorsRequest addMultipleAuthorRequests)
        {
            return this.ProduceResponse(await _authorService.AddRange(addMultipleAuthorRequests));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAuthorRequest model)
        {
            return this.ProduceResponse(await _authorService.Update(model));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            return this.ProduceResponse(await _authorService.Delete(id));
        }
    }
}
