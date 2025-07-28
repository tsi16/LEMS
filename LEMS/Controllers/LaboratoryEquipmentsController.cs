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
    public class LaboratoryEquipmentsController : Controller
    {
        private readonly LEMSContext _context;

        public LaboratoryEquipmentsController(LEMSContext context)
        {
            _context = context;
        }

        // GET: LaboratoryEquipments
        public async Task<IActionResult> Index()
        {
            var lEMSContext = _context.LaboratoryEquipments.Include(l => l.Department).Include(l => l.Equipment);
            return View(await lEMSContext.ToListAsync());
        }

        // GET: LaboratoryEquipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laboratoryEquipment = await _context.LaboratoryEquipments
                .Include(l => l.Department)
                .Include(l => l.Equipment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laboratoryEquipment == null)
            {
                return NotFound();
            }

            return View(laboratoryEquipment);
        }

        // GET: LaboratoryEquipments/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            ViewData["EquipmentId"] = new SelectList(_context.Equipments, "Id", "Id");
            return View();
        }

        // POST: LaboratoryEquipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DepartmentId,EquipmentId,IsActive,IsDeleted")] LaboratoryEquipment laboratoryEquipment)
        {
            try
            {
                _context.Add(laboratoryEquipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", laboratoryEquipment.DepartmentId);
                ViewData["EquipmentId"] = new SelectList(_context.Equipments, "Id", "Id", laboratoryEquipment.EquipmentId);
                return View(laboratoryEquipment);
            }
        }

        // GET: LaboratoryEquipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laboratoryEquipment = await _context.LaboratoryEquipments.FindAsync(id);
            if (laboratoryEquipment == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", laboratoryEquipment.DepartmentId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipments, "Id", "Id", laboratoryEquipment.EquipmentId);
            return View(laboratoryEquipment);
        }

        // POST: LaboratoryEquipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepartmentId,EquipmentId,IsActive,IsDeleted")] LaboratoryEquipment laboratoryEquipment)
        {
            if (id != laboratoryEquipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laboratoryEquipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaboratoryEquipmentExists(laboratoryEquipment.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", laboratoryEquipment.DepartmentId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipments, "Id", "Id", laboratoryEquipment.EquipmentId);
            return View(laboratoryEquipment);
        }

        // GET: LaboratoryEquipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laboratoryEquipment = await _context.LaboratoryEquipments
                .Include(l => l.Department)
                .Include(l => l.Equipment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laboratoryEquipment == null)
            {
                return NotFound();
            }

            return View(laboratoryEquipment);
        }

        // POST: LaboratoryEquipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laboratoryEquipment = await _context.LaboratoryEquipments.FindAsync(id);
            if (laboratoryEquipment != null)
            {
                _context.LaboratoryEquipments.Remove(laboratoryEquipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaboratoryEquipmentExists(int id)
        {
            return _context.LaboratoryEquipments.Any(e => e.Id == id);
        }
    }
}
