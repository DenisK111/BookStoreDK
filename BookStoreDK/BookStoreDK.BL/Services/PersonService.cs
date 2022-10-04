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

        public async Task<AddPersonResponse> Add(AddPersonRequest person)
        {
            {
                var auth = await _repo.GetPersonByName(person.Name);

                if (auth != null)
                    return new AddPersonResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Author already exist"
                    };
                var personObject = _mapper.Map<Author>(person);
                var result = await _repo.Add(personObject);

                return new AddPersonResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Person = result,

                };
            }
        }

        public async Task<Person?> Delete(int modelId)
        {
            return await _repo.Delete(modelId);
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Person?> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<UpdatePersonResponse> Update(UpdatePersonRequest model)
        {
            var modelToUpdate = await GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new UpdatePersonResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist"
                };
            }
            var personObject = _mapper.Map<Author>(model);
            var result = await _repo.Update(personObject);

            return new UpdatePersonResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Person = result,
            };

        }
    }
}
