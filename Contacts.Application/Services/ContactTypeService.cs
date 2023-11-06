using Contacts.Helpers;
using Contacts.Domain.DTO;
using Contacts.Domain.Entities;
using Contacts.Domain.Interfaces.Application;
using Contacts.Domain.Interfaces.Repositories;
using Contacts.Domain.DTO.Model;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Contacts.Application.Services 
{
    public class ContactTypeService : IContactTypeService
    {
        private IRepositoryBase<ContactType> _repository { get; set; }
        private ILogService _logService { get; set; }
        public ContactTypeService(IRepositoryBase<ContactType> repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        public ContactType GetById(int id, string ip = "")
        {

            var obj = _repository.Query(x => x.Id == id).FirstOrDefault();
            if (obj == null)
            {
                throw new NotFoundException("Type not found!");
            }

            _logService.Save($"Consulted a Type by Id: {id}.", ip);
            return obj;

        }

        public void Delete(int id, string ip = "")
        {
            var consulta = GetById(id, ip);
            var beforeChange = JsonConvert.SerializeObject(consulta);

            _repository.Delete(consulta);
            _repository.SaveChanges();

            _logService.Save($"Deleted a Type.", ip, beforeChange);
        }

        public PaginatedList<ContactType> List(int start, int limit, string name, string sort, string direction, string ip = "")
        {
            name = (name ?? "").ToUpper();
            limit = (limit > 0 ? limit : 10);

            var consulta = _repository.Query(x => (String.IsNullOrEmpty(name) || x.Name.ToUpper().Contains(name)));

            var lista = new PaginatedList<ContactType>
            {
                TotalRegistros = consulta.Count(),
                Lista = consulta.FiltrarPaginado(start, limit, sort, direction).ToList(),
            };

            _logService.Save($"Consulted a List of Type.", ip);

            return lista;
        }


        public void Save(ContactTypeModel obj, string ip = "")
        {
            var beforeChange = "";

            if (obj == null)
            {
                throw new Exception("The object cannot be null!");
            }

            var create = new ContactType()
            {
                Name = obj.Name,
            };

            _repository.Save(create);
            _repository.SaveChanges();

            var afterChange = JsonConvert.SerializeObject(create);
            _logService.Save($"Created a Type.", ip, beforeChange, afterChange);
        }

        public void Update(int id, ContactTypeModel obj, string ip = "")
        {
            if (obj == null)
            {
                throw new Exception("The object cannot be null!");
            }

            if (id == 0)
            {
                throw new Exception("Provide the record's ID!");
            }
            var exist = GetById(id, "");

            var beforeChange = JsonConvert.SerializeObject(exist);

            exist.Name = obj.Name;
            _repository.Update(exist);
            _repository.SaveChanges();

            var afterChange = JsonConvert.SerializeObject(exist);
            _logService.Save($"Updated a Type.", ip, beforeChange, afterChange);

        }
    }
}
