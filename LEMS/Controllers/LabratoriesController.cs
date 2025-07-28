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
    public class LabratoriesController : Controller
    {
        private readonly LEMSContext _context;

        public LabratoriesController(LEMSContext context)
        {
            _context = context;
        }

        // GET: Labratories
        public async Task<IActionResult> Index()
        {
            var lEMSContext = _context.Labratories.Include(l => l.Department);
            return View(await lEMSContext.ToListAsync());
        }

        // GET: Labratories/Details/5
        public IActionResult Details(int id)
        {
            var lab = _context.Labratories
                .Include(l => l.LaboratoryEquipments)
                .ThenInclude(le => le.Equipment)
                .Include(l => l.Department)
                .FirstOrDefault(l => l.Id == id);

            if (lab == null) return NotFound();

            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
            ViewBag.Equipments = new SelectList(_context.Equipments, "Id", "Name");

            return View(lab);
        }

        // GET: Labratories/Create
        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Labratory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DepartmentId,Description,IsActive,IsDeleted")] Labratory labratory)
        {
            try
            {
                // If Department is a navigation property, remove it from validation
                ModelState.Remove("Department");

                if (ModelState.IsValid)
                {
                    _context.Add(labratory);
                    await _context.SaveChangesAsync();

                   
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
               
                ModelState.AddModelError(string.Empty, "An error occurred while saving. Please try again.");
            }

            
            ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name", labratory.DepartmentId);
            return View(labratory);
        }






        // GET: Labratories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labratory = await _context.Labratories.FindAsync(id);
            if (labratory == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", labratory.DepartmentId);
            return View(labratory);
        }

        // POST: Labratories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DepartmentId,Description,IsActive,IsDeleted")] Labratory labratory)
        {
            if (id != labratory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labratory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabratoryExists(labratory.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", labratory.DepartmentId);
            return View(labratory);
        }

        // GET: Labratories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labratory = await _context.Labratories
                .Include(l => l.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labratory == null)
            {
                return NotFound();
            }

            return View(labratory);
        }

        // POST: Labratories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labratory = await _context.Labratories.FindAsync(id);
            if (labratory != null)
            {
                _context.Labratories.Remove(labratory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabratoryExists(int id)
        {
            return _context.Labratories.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> EditLaboratoryEquipment(int id, int equipmentId, int departmentId, bool isActive)
        {
            var labEquip = await _context.LaboratoryEquipments.FindAsync(id);
            if (labEquip == null) return NotFound();

            labEquip.EquipmentId = equipmentId;
            labEquip.DepartmentId = departmentId;
            labEquip.IsActive = isActive;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var equip = await _context.Equipments.FindAsync(id);
            if (equip == null) return NotFound();

            _context.Equipments.Remove(equip);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddEquipmentToLaboratory(int labratoryId, int equipmentId, int departmentId, bool isActive)
        {
            var labEquip = new LaboratoryEquipment
            {
                LabratoryId = labratoryId,
                DepartmentId = departmentId,
                EquipmentId = equipmentId,
                IsActive = isActive,
                IsDeleted = false
            };

            _context.LaboratoryEquipments.Add(labEquip);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = labratoryId });
        }

        public IActionResult Equipment(int id)
        {
            var laboratoryEquipment = _context.LaboratoryEquipments
                .Include(x=>x.Labratory)
                .Include(x => x.Equipment)
                .Include(x => x.Department)
                .FirstOrDefault(x => x.Id == id);

            if (laboratoryEquipment == null) return NotFound();

            var entries =  _context.EquipmentEntries
                .Include(x=>x.Equipment)
                .Include(x => x.Labratory)
                .Where(e => e.EquipmentId == laboratoryEquipment.EquipmentId && e.LabratoryId == laboratoryEquipment.LabratoryId)
                .Include(e => e.ActionByNavigation)
                .ToList();

            ViewBag.Entries = entries;
            return View(laboratoryEquipment); 
        }

        [HttpPost]
        public async Task<IActionResult> AddEntry(EquipmentEntry entry)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized(); 
            }

            entry.ActionBy = userId.Value;             
            entry.IsActive = true;
            entry.IsDeleted = false;
            entry.WithdrawalQuantity = 0;             
            entry.EntryDate = entry.EntryDate == default ? DateTime.Now : entry.EntryDate;

            _context.EquipmentEntries.Add(entry);
            await _context.SaveChangesAsync();
            AdjustQuantity(entry.LabratoryId, entry.EquipmentId);

            return RedirectToAction("Details", new { id = entry.EquipmentId });
        }

        
        public IActionResult AddWithdrawal(Withdrawl withdrawl)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            // Get the entry
            var entry = _context.EquipmentEntries
                .Find(withdrawl.EntryId);
         

            if (entry == null) return NotFound();

            double remaining = entry.Quantity - entry.WithdrawalQuantity;
            if (withdrawl.Quantity > remaining)
            {
                ModelState.AddModelError("", "Withdraw quantity exceeds available stock.");
                return BadRequest("Not enough stock.");
            }

            
            withdrawl.ActionBy = userId.Value;
            withdrawl.IsActive = true;
            withdrawl.IsDeleted = false;

            
            _context.Withdrawls.Add(withdrawl);

           
            entry.WithdrawalQuantity += withdrawl.Quantity;
            _context.EquipmentEntries.Update(entry);

             _context.SaveChanges();
            AdjustQuantity(entry.LabratoryId, entry.EquipmentId);

           
            return Redirect(Request.Headers["Referer"].ToString());
        }

        private void AdjustQuantity(int labratoryId, int equipmentId)
        {
          
            var entryQuantity = _context.EquipmentEntries
                .Where(x => x.LabratoryId == labratoryId && x.EquipmentId == equipmentId && x.IsActive)
                .Sum(x => x.Quantity);

           
            var withdrawalQuantity = _context.Withdrawls
                .Where(x => x.IsActive &&
                            x.Entry.LabratoryId == labratoryId &&
                            x.Entry.EquipmentId == equipmentId &&
                            x.Entry.IsActive)
                .Sum(x => x.Quantity);

            
            var balance = entryQuantity - withdrawalQuantity;

           
            var labEquipment = _context.LaboratoryEquipments
                .FirstOrDefault(x => x.LabratoryId == labratoryId &&
                                     x.EquipmentId == equipmentId &&
                                     x.IsActive);

            if (labEquipment != null)
            {
                labEquipment.Quantity = balance;
                _context.Update(labEquipment);
                _context.SaveChanges();
            }
        }

    }
}

