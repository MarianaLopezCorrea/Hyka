using Hyka.Models;

namespace Hyka.Services
{
    public interface IHistoryService
    {
        public int Create(Record record);

        public IQueryable<Record> GetAll();

        public IEnumerable<Record> GetBetween(DateTime start, DateTime end);
    }
}
