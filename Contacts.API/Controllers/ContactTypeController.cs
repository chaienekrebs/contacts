using Contacts.Domain.DTOs;
using Contacts.Domain.Entities;
using Contacts.Helpers;
using Contacts.Domain.Interfaces.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contacts.Application.Services;
using Contacts.Domain.DTO.Model;

namespace Contacts.API.Controllers
{
    [Route("[controller]")]
    public class ContactTypeController : Controller
    {
        private IContactTypeService _contactTypeService;
        public ContactTypeController(IContactTypeService contactTypeService)
        {
            _contactTypeService = contactTypeService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                var obj = _contactTypeService.GetById(id, ip);
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

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                _contactTypeService.Delete(id, ip);

                return Json(ApiResponse.Success(true));
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

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Save([FromBody] ContactTypeModel obj)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                _contactTypeService.Save(obj, ip);
                return Json(ApiResponse.Success(true));
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

        [HttpPut("{id}")]
        [AllowAnonymous]
        public IActionResult Update(int id, [FromBody] ContactTypeModel obj)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                _contactTypeService.Update(id, obj,ip);
                return Json(ApiResponse.Success(true));
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult List(int start, int limit, string name, string sort, string direction)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                var lista = _contactTypeService.List(start, limit, name, sort, direction, ip);
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

    }
}
