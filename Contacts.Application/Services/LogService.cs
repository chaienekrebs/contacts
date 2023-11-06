using Contacts.Domain.Entities;
using Contacts.Domain.DTO;
using Contacts.Domain.Interfaces.Application;
using Contacts.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Contacts.Helpers;

namespace Contacts.Application.Services
{
    public class LogService : ILogService
    {
        public LogService(IRepositoryBase<Log> repository, IRepositoryBase<Setting> settingRepository)
        {

            _repository = repository;
            _settingRepository = settingRepository;
        }

        private IRepositoryBase<Log> _repository { get; set; }
        private IRepositoryBase<Setting> _settingRepository { get; set; }

        public void Save(string description, string ip, string beforeChange = "", string afterChange = "")
        {
            _repository.Save(new Log()
            {
                Description = description,
                Ip = ip,
                BeforeChange = beforeChange,
                AfterChange = afterChange,
                Date = DateTime.Now
            });
            _repository.SaveChanges();
        }

        public PaginatedList<Log> List(int start, int limit, string description, string sort, string direction, string ip = "")
        {
            description = (description ?? "").ToUpper();
            limit = (limit > 0 ? limit : 10);
            var consulta = _repository.Query(x => String.IsNullOrEmpty(description) || x.Description.ToUpper().Contains(description));

            var lista = new PaginatedList<Log>
            {
                TotalRegistros = consulta.Count(),
                Lista = consulta.FiltrarPaginado(start, limit, sort, direction).ToList(),
            };

            Save($"Consulted a List of Log.", ip);

            return lista;
        }

        public List<IpInfo> GetById(int id, string ip)
        {
            Setting setting = _settingRepository.QueryAll().FirstOrDefault();
            if (setting != null)
            {

                string accessKey = setting.AccessKey;
                string apiUrl = "http://apiip.net/api/check";

                var log = _repository.Query(x => x.Id == id).FirstOrDefault(); ;
                if (log == null)
                {
                    throw new NotFoundException("Log not found!");
                }


                List<IpInfo> results = new List<IpInfo>();
                using (HttpClient client = new HttpClient())
                {
                    var ipSearch = log.Ip;
                    string url = $"{apiUrl}?ip={ipSearch}&accessKey={accessKey}";

                    try
                    {
                        HttpResponseMessage response = client.GetAsync(url).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            if (!String.IsNullOrEmpty(responseBody))
                            {
                                IpInfo ipInfo = JsonConvert.DeserializeObject<IpInfo>(responseBody);
                                results.Add(ipInfo);
                            }
                        }
                        else
                        {
                            throw new NotFoundException("Ip not found!");
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("There was an issue fetching the data!");
                    }

                }

                Save($"Consulted a Log by Id: {id}", ip);
                return results;
            }
            else
            {
                throw new Exception("Settings not found!");
            }

        }

    }
}
