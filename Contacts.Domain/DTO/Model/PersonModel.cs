using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Domain.DTO.Model
{
    public class PersonModel
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Address { get; set; }
    }
}
