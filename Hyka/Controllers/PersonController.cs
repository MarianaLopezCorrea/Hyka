using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using Hyka.Data;
using Microsoft.AspNetCore.Authorization;

namespace Hyka.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PersonController : ControllerBase

    {
        private readonly ApplicationDbContext _db;

        public PersonController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "GetPerson")]
        public IActionResult Get()
        {
            return Ok(_db.People);
        }

    }
}