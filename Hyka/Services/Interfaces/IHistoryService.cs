using Hyka.Models;

namespace Hyka.Services
{
    public interface IHistoryService
    {
        public int Create(History history);

        public IQueryable<History> GetBetween(DateTime start, DateTime end);
    }
}
