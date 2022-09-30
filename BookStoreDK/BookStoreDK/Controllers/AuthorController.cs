using BookStoreDK.BL.Interfaces;
using BookStoreDK.Models.Models;
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

        [HttpGet]
        public IEnumerable<Author> Get()
        {

            return _authorService.GetAll();

        }

        [HttpGet(nameof(GetById))]

        public Author? GetById(int Id)
        {
            return _authorService.GetById(Id);

        }

        [HttpPost]
        public Author? Add([FromBody] Author model)
        {
            return _authorService.Add(model);


        }

        [HttpPut]
        public Author? Update([FromBody] Author model)
        {
            return _authorService.Update(model);
        }

        [HttpDelete]
        public Author? Delete([FromBody] int id)
        {
            return _authorService.Delete(id);
        }
    }
}
