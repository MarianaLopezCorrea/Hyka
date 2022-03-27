using Microsoft.AspNetCore.Mvc;
using Hyka.Service.TerritoryDto;
using Hyka.Models;

namespace Hyka.Service.Controllers
{
    [ApiController]
    // The URL where the controller going to send data https://localhost:0000/name
    [Route("api/territories")]
    public class TerritoryController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<TerritoryRecord> Get()
        {
            return Territory.territories;
        }


        [HttpGet("{departmentId}/{municipalityId}")]
        public TerritoryRecord Get(string departmentId, string municipalityId)
        {
            // Get the unique bracelet that meets that condition
            var TerritoryDto = Territory.territories.Where(
                TerritoryDto =>
             TerritoryDto.DepartmentId == departmentId &&
             TerritoryDto.MunicipalityID == municipalityId
             )
             .FirstOrDefault();
            return TerritoryDto != null ? TerritoryDto : Territory.territories[0];
        }


    }
}