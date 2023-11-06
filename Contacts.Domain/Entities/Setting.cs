using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Domain.Entities
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string AccessKey { get; set; }
    }
}
