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

        // GET
        public IActionResult Create()
        {
            return View();
        }
        // POST
        [HttpPost]
        // Prevent request forgery 
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Blockbuster obj)
        {
            // if (obj.Name == obj.DisplayOrder.ToString())
            // {
            //     ModelState.AddModelError("Name", "Name can't be equal to Display Order ");
            // }

            if (ModelState.IsValid)
            {
                _db.Blockbusters.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Blockbuster Created Correctly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET
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
        // POST 
        [HttpPost]
        // Prevent request falsification wi
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Blockbuster obj)
        {
            // if (obj.Name == obj.DisplayOrder.ToString())
            // {
            //     ModelState.AddModelError("Name", "Name can't be equal to Display Order ");
            // }

            if (ModelState.IsValid)
            {
                _db.Blockbusters.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Blockbuster Updated Correctly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET
        public IActionResult Delete(int? id)
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
        // POST 
        [HttpPost, ActionName("Delete")]
        // Prevent request falsification wi
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
