using Microsoft.AspNetCore.Mvc;

using Hyka.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Areas.Identity.PoliciesDefinition;

namespace Hyka.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Policy = Policy.REQUIRE_BLOCKBUSTER)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : Controller

    {
        private readonly ApplicationDbContext _db;

        public PersonController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_db.People);
        }

    }
}