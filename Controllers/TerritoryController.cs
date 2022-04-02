using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using Hyka.Data;

namespace Hyka.Service.Controllers
{
    [ApiController]
    // The URL where the controller going to send data /api/territories
    [Route("api/territories")]
    public class TerritoryController : ControllerBase

    {
        private readonly ApplicationDbContext _db;

     

        public TerritoryController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_db.Territories);
        }


        [HttpGet("{daneId}")]
        public IActionResult GetById(string daneId)
        {       
            var territory = _db.Territories
            .Where(t => t.DaneId == daneId)
            .FirstOrDefault();
            return territory != null ? Ok(territory) : NotFound();
        }

    }
}