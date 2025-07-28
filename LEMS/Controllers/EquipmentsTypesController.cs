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
    public class EquipmentsTypesController : Controller
    {
        private readonly LEMSContext _context;

        public EquipmentsTypesController(LEMSContext context)
        {
            _context = context;
        }

        // GET: EquipmentsTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.EquipmentsTypes.ToListAsync());
        }

        // GET: EquipmentsTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentsType = await _context.EquipmentsTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipmentsType == null)
            {
                return NotFound();
            }

            return View(equipmentsType);
        }

        // GET: EquipmentsTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EquipmentsTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive,IsDeleted")] EquipmentsType equipmentsType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipmentsType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipmentsType);
        }

        // GET: EquipmentsTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentsType = await _context.EquipmentsTypes.FindAsync(id);
            if (equipmentsType == null)
            {
                return NotFound();
            }
            return View(equipmentsType);
        }

        // POST: EquipmentsTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive,IsDeleted")] EquipmentsType equipmentsType)
        {
            if (id != equipmentsType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipmentsType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentsTypeExists(equipmentsType.Id))
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
            return View(equipmentsType);
        }

        // GET: EquipmentsTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentsType = await _context.EquipmentsTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipmentsType == null)
            {
                return NotFound();
            }

            return View(equipmentsType);
        }

        // POST: EquipmentsTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipmentsType = await _context.EquipmentsTypes.FindAsync(id);
            if (equipmentsType != null)
            {
                _context.EquipmentsTypes.Remove(equipmentsType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentsTypeExists(int id)
        {
            return _context.EquipmentsTypes.Any(e => e.Id == id);
        }
    }
}
