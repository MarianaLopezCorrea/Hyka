using Hyka.Data;
using Hyka.Models;

namespace Hyka.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly ApplicationDbContext _db;

        public HistoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public int Create(Record record)
        {
            _db.History.Add(record);
            return _db.SaveChanges();
        }

        public IQueryable<Record> GetAll()
        {
            return _db.History.AsQueryable();
        }

        public IEnumerable<Record> GetBetween(DateTime start, DateTime end)
        {
            return _db.History.Where(h =>
                h.VisitDateTime.Date >= start.Date && h.VisitDateTime.Date <= end.Date
            );
        }

    }
}