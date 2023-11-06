using Contacts.Helpers;
using Contacts.Domain.DTO;
using Contacts.Domain.Entities;
using Contacts.Domain.Interfaces.Application;
using Contacts.Domain.Interfaces.Repositories;
using Contacts.Domain.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Contacts.Application.Services
{
    public class PersonService : IPersonService
    {
        private IRepositoryBase<Person> _repository { get; set; }
        private ILogService _logService { get; set; }
        public PersonService(IRepositoryBase<Person> repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        public Person GetById(int id, string ip="")
        {
            var obj = _repository.Query(x => x.Id == id).FirstOrDefault();
            if (obj == null)
            {
                throw new NotFoundException("Person not found!");
            }

            _logService.Save($"Consulted a Person by Id: {id}", ip);
            return obj;

        }

        public void Delete(int id, string ip = "")
        {
            var consulta = GetById(id, ip);
            var beforeChange = JsonConvert.SerializeObject(consulta);

            _repository.Delete(consulta);
            _repository.SaveChanges();

            _logService.Save($"Deleted a Person.", ip, beforeChange);
        }

        public PaginatedList<Person> List(int start, int limit, string name, string cpf, string sort, string direction, string ip = "")
        {

            name = (name ?? "").ToUpper();
            limit = (limit > 0 ? limit : 10);

            var consulta = _repository.Query(x => (String.IsNullOrEmpty(name) || x.Name.ToUpper().Contains(name)) && (String.IsNullOrEmpty(cpf) || x.Cpf.Contains(cpf)));

            var lista = new PaginatedList<Person>
            {
                TotalRegistros = consulta.Count(),
                Lista = consulta.FiltrarPaginado(start, limit, sort, direction).ToList(),
            };

            _logService.Save($"Consulted a List of Person.", ip);

            return lista;

        }
        public void Save(PersonModel obj, string ip = "")
        {
            if (obj == null)
            {
                throw new Exception("The object cannot be null!");
            }
            var beforeChange = "";

            var create = new Person()
            {
                Cpf = obj.Cpf,
                Address = obj.Address,
                Name = obj.Name,
            };

            _repository.Save(create);
            _repository.SaveChanges();

            var afterChange = JsonConvert.SerializeObject(create);

            _logService.Save($"Created a Person.", ip, beforeChange, afterChange);

        }

        public void Update(int id, PersonModel obj, string ip = "")
        {
            if (obj == null)
            {
                throw new Exception("The object cannot be null!");
            }

            if (id == 0)
            {
                throw new Exception("Provide the record's ID!");
            }
            var exist = GetById(id,"");
            var beforeChange = JsonConvert.SerializeObject(exist);

            exist.Cpf = obj.Cpf;
            exist.Name = obj.Name;
            exist.Address = obj.Address;

            _repository.Update(exist);
            _repository.SaveChanges();

            var afterChange = JsonConvert.SerializeObject(exist);
            _logService.Save($"Updated a Person.", ip, beforeChange, afterChange);
        }
    }
}
