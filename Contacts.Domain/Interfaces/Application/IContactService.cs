using Contacts.Domain.DTO;
using Contacts.Domain.DTO.Model;
using Contacts.Domain.Entities;

namespace Contacts.Domain.Interfaces.Application
{
    public interface IContactService
    {
        Contact GetById(int id, string ip="");
        PaginatedList<Contact> List(int start, int limit, int personId, int contactTypeId, string value, string sort, string direction, string ip = "");
        void Save(ContactModel obj, string ip = "");
        void Delete(int id, string ip = "");
        void Update(int id, ContactModel obj, string ip = "");
    }
}
