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
        public IEnumerable<Territory> Get()
        {
            return _db.Territories;
        }


        [HttpGet("{daneId}")]
        public ActionResult<Territory> Get(string daneId)
        {
            // Get the unique bracelet that meets that condition
            var territory = _db.Territories
            .Where(t => t.DaneId == daneId)
            .FirstOrDefault();
            return territory != null ? territory : NotFound();
        }

    }
}