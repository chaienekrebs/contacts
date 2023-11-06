using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Domain.Entities
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string BeforeChange { get; set; }
        public string AfterChange { get; set; }
    }
}
