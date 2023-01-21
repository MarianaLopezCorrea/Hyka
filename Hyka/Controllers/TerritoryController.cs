using Microsoft.AspNetCore.Mvc;

using Hyka.Data;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Hyka.Models;
using Hyka.Services;

namespace Hyka.Service.Controllers
{
    [ApiController]
    [Route("api/territories")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class TerritoryController : ControllerBase

    {
        private readonly ITerritoryService _territoryService;

        public TerritoryController(ITerritoryService territoryService)
        {
            _territoryService = territoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_territoryService.Get());
        }

        [HttpGet("{daneId}")]
        public IActionResult GetById(string daneId)
        {
            if (daneId == null)
                return BadRequest();
            var territory = _territoryService.GetById(daneId);
            return territory != null ? Ok(territory) : NotFound();
        }
    }
}