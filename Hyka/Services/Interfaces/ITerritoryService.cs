using Hyka.Models;

namespace Hyka.Services
{
    public interface ITerritoryService
    {
        public IEnumerable<Territory> Get();

        public Territory GetById(String daneId);

    }
}
