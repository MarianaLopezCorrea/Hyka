using Hyka.Data;
using Hyka.Models;

namespace Hyka.Services
{
    public class TerritoryService : ITerritoryService
    {
        private readonly ApplicationDbContext _db;

        public TerritoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Territory> Get()
        {
            return _db.Territories.AsEnumerable();
        }

        public Territory GetById(String id)
        {
            return _db.Territories.Find(id);
        }

    }
}