using System.Net;
using AutoMapper;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public AddPersonResponse Add(AddPersonRequest person)
        {
            {
                var auth = _repo.GetPersonByName(person.Name);

                if (auth != null)
                    return new AddPersonResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Author already exist"
                    };
                var personObject = _mapper.Map<Author>(person);
                var result = _repo.Add(personObject);

                return new AddPersonResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Person = result,

                };
            }
        }

        public Person? Delete(int modelId)
        {
            return _repo.Delete(modelId);
        }

        public IEnumerable<Person> GetAll()
        {
            return _repo.GetAll();
        }

        public Person? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public UpdatePersonResponse Update(UpdatePersonRequest model)
        {
            var modelToUpdate = GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new UpdatePersonResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist"
                };
            }
            var personObject = _mapper.Map<Author>(model);
            var result = _repo.Update(personObject);

            return new UpdatePersonResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Person = result,
            };

        }
    }
}
