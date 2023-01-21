using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Hyka.Services;

namespace Hyka.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : Controller
    {

        private readonly IPersonService _personService;
        

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            if (id == null)
                return BadRequest();
            var person = _personService.GetById(id);
            return person != null ? Ok(person) : NotFound();
        }
    }
}