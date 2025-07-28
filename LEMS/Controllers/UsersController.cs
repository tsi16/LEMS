using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LEMS.Models;
using System.Data;
using LEMS.ViewModels;
using LEMS.Utilities;

namespace LEMS.Controllers
{
    public class UsersController : Controller
    {
        private readonly LEMSContext _context;
        private readonly UserManagement _um;
        private readonly IHttpContextAccessor _httpContext;

        
        public UsersController(LEMSContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
           
            _httpContext = httpContext;
        }

   



// GET: Users
public async Task<IActionResult> Index()
        {
            var lEMSContext = _context.Users.Include(u => u.Gender);
            return View(await lEMSContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users
                .Include(u => u.LabratoryAssignmentUsers)
                .ThenInclude(a => a.Labratory)
                .Include(u => u.Gender)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            ViewBag.Labs = new SelectList(_context.Labratories.Where(l => !l.IsDeleted), "Id", "Name");
            return View(user);
        }
        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GenderId,FirstName,MiddleName,LastName,Email,Password,UserName,PhoneNumber,CreatedDate,LastLogon,FailureCount,BlockEndDate,DefaultLanguageId,IsActive,IsDeleted")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id", user.GenderId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id", user.GenderId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenderId,FirstName,MiddleName,LastName,Email,Password,UserName,PhoneNumber,CreatedDate,LastLogon,FailureCount,BlockEndDate,DefaultLanguageId,IsActive,IsDeleted")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id", user.GenderId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

       
        public async Task<IActionResult> AssignLab(int userId, int labratoryId)
        {
            var existing = await _context.LabratoryAssignments
                .FirstOrDefaultAsync(x => x.UserId == userId && x.LabratoryId == labratoryId && !x.IsDeleted);

            if (existing != null)
            {
                
                return RedirectToAction("Details", new { id = userId });
            }

            var actionBy = HttpContext.Session.GetInt32("UserId");
            if (actionBy == null) return Unauthorized();

            var assignment = new LabratoryAssignment
            {
                UserId = userId,
                LabratoryId = labratoryId,
                AssignedDate = DateTime.Now,
                ActionBy = actionBy.Value,
                IsActive = true,
                IsDeleted = false
            };

            _context.LabratoryAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            //return RedirectToAction("Details", new { id = userId });
            return Redirect(Request.Headers["Referer"].ToString());
        }


    }
}
