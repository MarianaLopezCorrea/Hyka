using Hyka.Data;
using Hyka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hyka.Controllers
{
    [Authorize]
    public class BlockbusterController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BlockbusterController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Blockbuster> objBlockbusterList = _db.Blockbusters;
            return View(objBlockbusterList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Blockbuster obj)
        {
            if (ModelState.IsValid)
            {
                await _db.Blockbusters.AddAsync(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Blockbuster Created Correctly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var BlockbusterFromDb = _db.Blockbusters.Find(id);
            //var BlockbusterFromDbFirst = _db.Blockbusters.FirstOrDefault(c => c.Id == id);
            //var BlockbusterFromDbSingle = _db.Blockbusters.SingleOrDefault(c => c.Id == id);
            return BlockbusterFromDb == null ?
                NotFound() : View(BlockbusterFromDb);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Blockbuster obj)
        {
            if (ModelState.IsValid)
            {
                _db.Blockbusters.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Blockbuster Updated Correctly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return BadRequest();

            var BlockbusterFromDb = _db.Blockbusters.Find(id);
            return BlockbusterFromDb == null ?
                NotFound() : View(BlockbusterFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Blockbusters.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Blockbusters.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Blockbuster Deleted Correctly";
            return RedirectToAction("Index");
        }

    }
}
