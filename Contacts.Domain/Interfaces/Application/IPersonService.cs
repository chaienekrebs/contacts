using Contacts.Domain.DTO;
using Contacts.Domain.DTO.Model;
using Contacts.Domain.Entities;

namespace Contacts.Domain.Interfaces.Application
{
    public interface IPersonService
    {
        Person GetById(int id, string ip = "");
        PaginatedList<Person> List(int start, int limit, string name, string cpf, string sort, string direction, string ip = "");
        void Save(PersonModel obj, string ip = "");
        void Delete(int id, string ip = "");
        void Update(int id, PersonModel obj, string ip = "");
    }
}
