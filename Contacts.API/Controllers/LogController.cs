using Contacts.Domain.DTOs;
using Contacts.Domain.Entities;
using Contacts.Helpers;
using Contacts.Domain.Interfaces.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contacts.Domain.DTO.Model;

namespace Contacts.API.Controllers
{
    [Route("[controller]")]
    public class LogController : Controller
    {
        private ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult List(int start, int limit, string description, string sort, string direction)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                var lista = _logService.List(start, limit, description, sort, direction, ip);
                return Json(new
                {
                    status = true,
                    recordsFiltered = lista.TotalRegistros,
                    recordsTotal = lista.TotalRegistros,
                    data = lista.Lista,
                    dataCounts = lista.TotalRegistros,
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(ApiResponse.Error(e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse.Error(e.Message));
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                var obj = _logService.GetById(id, ip);
                return Json(ApiResponse.Success(obj));
            }
            catch (NotFoundException e)
            {
                return NotFound(ApiResponse.Error(e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse.Error(e.Message));
            }
        }

    }
}
