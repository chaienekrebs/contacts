using Contacts.Domain.DTO;
using Contacts.Domain.DTO.Model;
using Contacts.Domain.Entities;

namespace Contacts.Domain.Interfaces.Application
{
    public interface IContactTypeService
    {
        ContactType GetById(int id, string ip = "");
        PaginatedList<ContactType> List(int start, int limit, string name, string sort, string direction, string ip = "");
        void Save(ContactTypeModel obj, string ip = "");
        void Delete(int id, string ip = "");
        void Update(int id, ContactTypeModel obj, string ip = "");
    }
}
