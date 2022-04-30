using Hyka.Data;
using Hyka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
            IEnumerable<Blockbuster> blockbusterBlockbusterList = _db.Blockbusters;
            return View(blockbusterBlockbusterList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Blockbuster blockbuster)
        {
            var userExists = await _db.Blockbusters.FindAsync(blockbuster.Id);

            if (userExists != null)
            {
                ModelState.AddModelError("Id", "User already exists");
            }

            if (ModelState.IsValid)
            {
                await _db.Blockbusters.AddAsync(blockbuster);
                await _db.SaveChangesAsync();
                TempData["success"] = "Blockbuster Created Correctly";
                return RedirectToAction("Index");
            }
            return View(blockbuster);
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
        public IActionResult Edit(Blockbuster blockbuster)
        {
            // if (blockbuster.Name == blockbuster.DisplayOrder.ToString())
            // {
            //     ModelState.AddModelError("Name", "Name can't be equal to Display Order ");
            // }

            if (ModelState.IsValid)
            {
                _db.Blockbusters.Update(blockbuster);
                _db.SaveChanges();
                TempData["success"] = "Blockbuster Updated Correctly";
                return RedirectToAction("Index");
            }
            return View(blockbuster);
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
            var blockbuster = _db.Blockbusters.Find(id);
            if (blockbuster == null)
            {
                return NotFound();
            }

            _db.Blockbusters.Remove(blockbuster);
            _db.SaveChanges();
            TempData["success"] = "Blockbuster Deleted Correctly";
            return RedirectToAction("Index");
        }

    }
}
