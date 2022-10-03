using BookStoreDK.BL.Interfaces;
using BookStoreDK.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {

        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personRepo;

        public PersonController(ILogger<PersonController> logger, IPersonService userRepo)
        {
            _logger = logger;
            _personRepo = userRepo;
        }

        [HttpGet]
        public IEnumerable<Person> Get()
        {

            return _personRepo.GetAll();

        }

        [HttpGet(nameof(GetById))]

        public Person? GetById(int Id)
        {
            return _personRepo.GetById(Id);

        }

        [HttpPost]
        public Person? Add([FromBody] Person model)
        {
            return _personRepo.Add(model);


        }

        [HttpPut]
        public Person? Update([FromBody] Person model)
        {
            return _personRepo.Update(model);
        }

        [HttpDelete]
        public Person? Delete([FromBody] int id)
        {
            return _personRepo.Delete(id);
        }
    }
}