using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Domain.Entities
{
    public class ContactType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
