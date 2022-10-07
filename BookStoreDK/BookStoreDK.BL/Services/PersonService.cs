using System.Net;
using AutoMapper;
using BookStoreDK.BL.Helpers;
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

        public async Task<PersonResponse> Add(AddPersonRequest person)
        {
            {
                var auth = await _repo.GetPersonByName(person.Name);

                if (auth != null)
                    return new PersonResponse()
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "Author already exist"
                    };
                var personObject = _mapper.Map<Author>(person);
                var result = await _repo.Add(personObject);

                return new PersonResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Model = result,

                };
            }
        }

        public async Task<PersonResponse> Delete(int modelId)
        {
            var result = await _repo.Delete(modelId);
            return NullChecker.CheckForNullObjectAndReturnResponse<Person,PersonResponse>(result, "Id does not exist");
        }

        public async Task<PersonCollectionResponse> GetAll()
        {
            var result = await _repo.GetAll();

            return new PersonCollectionResponse()
            {
                Model = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public async Task<PersonResponse> GetById(int id)
        {
            var result = await _repo.GetById(id);
            return NullChecker.CheckForNullObjectAndReturnResponse<Person, PersonResponse>(result, "Id does not exist");
        }

        public async Task<PersonResponse> Update(UpdatePersonRequest model)
        {
            var modelToUpdate = await GetById(model.Id);

            if (modelToUpdate == null)
            {
                return new PersonResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist"
                };
            }
            var personObject = _mapper.Map<Author>(model);
            var result = await _repo.Update(personObject);

            return new PersonResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = result,
            };

        }
        
    }
}
