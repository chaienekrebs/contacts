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
    public class PersonController : Controller
    {
        private IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                var obj = _personService.GetById(id, ip);
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
                _personService.Delete(id, ip);

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
        public IActionResult Save([FromBody] PersonModel obj)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                _personService.Save(obj, ip);
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
        public IActionResult Update(int id, [FromBody] PersonModel obj)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                _personService.Update(id, obj, ip);
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
        public IActionResult List(int start, int limit, string name, string cpf, string sort, string direction)
        {
            try
            {
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                var lista = _personService.List(start, limit, name, cpf, sort, direction, ip);
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
