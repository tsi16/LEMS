using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LEMS.Models;

namespace LEMS.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly LEMSContext _context;

        public EquipmentsController(LEMSContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index()
        {
            var equipments = await _context.Equipments.Include(e => e.EquipmentsType).ToListAsync();
            ViewBag.Role = HttpContext.Session.GetString("RoleName");
            return View(equipments);
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .Include(e => e.EquipmentsType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            ViewData["EquipmentsTypeId"] = new SelectList(_context.EquipmentsTypes, "Id", "Id");
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,MeasurmentUnit,Code,EquipmentsTypeId,PhotoUrl,Discription,IsActive,IsDeleted")] Equipment equipment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(equipment);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["EquipmentsTypeId"] = new SelectList(_context.EquipmentsTypes, "Id", "Id", equipment.EquipmentsTypeId);
        //    return View(equipment);
        //}
     


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MeasurmentUnit,Code,EquipmentsTypeId,PhotoUrl,Discription,IsActive,IsDeleted")] Equipment equipment)
        {
            try
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Optional: log the error
                ModelState.AddModelError(string.Empty, "An error occurred while saving the equipment.");

                ViewData["EquipmentsTypeId"] = new SelectList(_context.EquipmentsTypes, "Id", "Id", equipment.EquipmentsTypeId);
                return View(equipment);
            }
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            ViewData["EquipmentsTypeId"] = new SelectList(_context.EquipmentsTypes, "Id", "Id", equipment.EquipmentsTypeId);
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MeasurmentUnit,Code,EquipmentsTypeId,PhotoUrl,Discription,IsActive,IsDeleted")] Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipmentsTypeId"] = new SelectList(_context.EquipmentsTypes, "Id", "Id", equipment.EquipmentsTypeId);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .Include(e => e.EquipmentsType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipments.Remove(equipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipments.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(int id, IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return RedirectToAction("Details", new { id });

            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
                return NotFound();

            // Save to wwwroot/uploads/equipment/
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/equipment");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            // Update PhotoUrl
            equipment.PhotoUrl = "/uploads/equipment/" + uniqueFileName;
            _context.Update(equipment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImage(int id, IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                ModelState.AddModelError("photo", "Please select an image file.");
                var equipment = await _context.Equipments.FindAsync(id);
                return View("Details", equipment);
            }

            var equipmentFromDb = await _context.Equipments.FindAsync(id);
            if (equipmentFromDb == null)
            {
                return NotFound();
            }

            // Save the new photo file
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/equipments");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            // Delete old image if exists
            if (!string.IsNullOrEmpty(equipmentFromDb.PhotoUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", equipmentFromDb.PhotoUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Update the photo URL
            equipmentFromDb.PhotoUrl = "/images/equipments/" + uniqueFileName;

            _context.Update(equipmentFromDb);
            await _context.SaveChangesAsync();

            // Redirect back to Details page to see the new image
            return RedirectToAction("Details", new { id = id });
        }


    }
}
