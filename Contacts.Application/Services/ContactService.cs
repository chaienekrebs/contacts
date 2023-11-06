using Contacts.Helpers;
using Contacts.Domain.DTO;
using Contacts.Domain.Entities;
using Contacts.Domain.Interfaces.Application;
using Contacts.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Contacts.Domain.DTO.Model;
using Newtonsoft.Json;

namespace Contacts.Application.Services
{
    public class ContactService : IContactService
    {
        private IRepositoryBase<Contact> _repository { get; set; }
        private IContactTypeService _contactTypeService;
        private IPersonService _personService;
        private ILogService _logService { get; set; }

        public ContactService(IRepositoryBase<Contact> repository, IContactTypeService contactTypeService, IPersonService personService, ILogService logService)
        {
            _repository = repository;
            _contactTypeService = contactTypeService;
            _personService = personService;
            _logService = logService;
        }

        public Contact GetById(int id, string ip = "")
        {

            var obj = _repository.Query(x => x.Id == id).Include(x => x.Person).Include(x => x.ContactType).FirstOrDefault();
            if (obj == null)
            {
                throw new NotFoundException("Contact not found!");
            }

            _logService.Save($"Consulted a Contact by Id: {id}", ip);

            return obj;

        }

        public void Delete(int id, string ip = "")
        {
            var consulta = GetById(id, ip);
            var beforeChange = JsonConvert.SerializeObject(consulta);

            _repository.Delete(consulta);
            _repository.SaveChanges();

            _logService.Save($"Deleted a Contact.", ip, beforeChange);

        }

        public PaginatedList<Contact> List(int start, int limit, int personId, int contactTypeId, string value, string sort, string direction, string ip = "")
        {
            value = (value ?? "").ToUpper();
            limit = (limit > 0 ? limit : 10);
            var consulta = _repository.Query(x => (personId == 0 || x.PersonId == personId) && (contactTypeId == 0 || x.ContactTypeId == contactTypeId) && (String.IsNullOrEmpty(value) || x.Value.ToUpper().Contains(value)))
                .Include(x => x.Person)
                .Include(x => x.ContactType);

            var lista = new PaginatedList<Contact>
            {
                TotalRegistros = consulta.Count(),
                Lista = consulta.FiltrarPaginado(start, limit, sort, direction).ToList(),
            };

            _logService.Save($"Consulted a List of Contact.", ip);

            return lista;
        }


        public void Save(ContactModel obj, string ip = "")
        {
            var beforeChange = "";
            if (obj == null)
            {
                throw new Exception("The object cannot be null!");
            }

            _contactTypeService.GetById(obj.ContactTypeId, ip);
            _personService.GetById(obj.PersonId,ip);

            var create = new Contact()
            {
                ContactTypeId = obj.ContactTypeId,
                PersonId = obj.PersonId,
                Value = obj.Value,
            };

            _repository.Save(create);
            _repository.SaveChanges();

            var afterChange = JsonConvert.SerializeObject(create);
            _logService.Save($"Created a Contact.", ip, beforeChange, afterChange);
        }

        public void Update(int id, ContactModel obj, string ip = "")
        {
            if (obj == null)
            {
                throw new Exception("The object cannot be null!");
            }

            if (id == 0)
            {
                throw new Exception("Provide the record's ID!");
            }

            _contactTypeService.GetById(obj.ContactTypeId,ip);
            _personService.GetById(obj.PersonId,ip);

            var exist = GetById(id, ip);
            var beforeChange = JsonConvert.SerializeObject(exist);

            exist.ContactTypeId = obj.ContactTypeId;
            exist.PersonId = obj.PersonId;
            exist.Value = obj.Value;

            _repository.Update(exist);
            _repository.SaveChanges();

            var afterChange = JsonConvert.SerializeObject(exist);
            _logService.Save($"Updated a Contact.", ip, beforeChange, afterChange);
        }
    }
}
