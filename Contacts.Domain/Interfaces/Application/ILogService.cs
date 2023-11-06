using Contacts.Domain.DTO;
using Contacts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Domain.Interfaces.Application
{
    public interface ILogService
    {
        void Save(string description, string ip, string beforeChange = "", string afterChange = "");
        PaginatedList<Log> List(int start, int limit, string description, string sort, string direction, string ip = "");
        List<IpInfo> GetById(int id, string ip);
    }
}
