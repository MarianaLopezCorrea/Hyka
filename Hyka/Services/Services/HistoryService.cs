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

        public int Create(History history)
        {
            _db.Histories.Add(history);
            return _db.SaveChanges();
        }

        public IQueryable<History> GetBetween(DateTime start, DateTime end)
        {
            return _db.Histories.Where(h =>
                h.VisitDateTime.Date >= start.Date && h.VisitDateTime.Date <= end.Date
                );
        }
    }
}