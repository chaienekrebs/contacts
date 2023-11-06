using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Domain.Entities
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public int ContactTypeId { get; set; }
        public int PersonId { get; set; }
        public string Value { get; set; }

        public ContactType ContactType { get; set; }
        public Person Person { get; set; }
    }
}
