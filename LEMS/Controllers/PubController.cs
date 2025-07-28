using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LEMS.Models;
using System.Data;
using LEMS.ViewModels;
using LEMS.Utilities;
using hu_utils;

namespace LEMS.Controllers
{
    public class PubController : Controller
    {
        private readonly LEMSContext _context;
        private readonly UserManagement _um;
        private readonly IHttpContextAccessor _httpContext;

        public PubController(LEMSContext context, UserManagement um , IHttpContextAccessor httpContext)
        {
            _context = context;
            _um = um;
            _httpContext = httpContext;
        }

        public ActionResult Labs()
        {
            var labs =  _context.Labratories
                .Include(l => l.Department)
                //.Where(l => l.IsActive && !l.IsDeleted)
                .ToList();

            return View(labs);
        }

        public ActionResult Equipment(string search)
        {
            var equipment = _context.Equipments
                .Include(e => e.EquipmentsType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                equipment = equipment.Where(e => e.Name.Contains(search));
                ViewBag.SearchTerm = search;
            }

            return View(equipment.ToList());
        }

        public ActionResult Departments()
        {
            var departments =  _context.Departments
               .Include(d => d.Branch)
               //.Where(d => d.IsActive && !d.IsDeleted)
               .ToList();

            return View(departments);
        }

        public ActionResult Contact(int id)
        {
            var assignments = _context.LabratoryAssignments
               .Include(a => a.User)
               .Include(a => a.Labratory)
                   .ThenInclude(l => l.Department)
               .Where(a => a.IsActive && !a.IsDeleted)
               .ToList();

            return View(assignments);
        }

        public ActionResult About(int id) => View();

        [HttpGet]
        public ActionResult LogIn()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult LogIn(LoginViewModel login, string Headedto, string FingerPrint, string Browser, string Platform, string TimeZone, string UserAgent)
        {
            if (Headedto != null) TempData["Headedto"] = Headedto;
            var users = _context.Users
                .Where(x => x.UserName == login.UserName)
                .Include(x => x.UserRoles);
            var user = users.FirstOrDefault();
            if (user != null)
            {

                if (user.BlockEndDate > DateTime.Now)
                {
                    TempData["Error"] = "Your account has been blocked until " + user.BlockEndDate + " due to multiple login failure.";
                    return RedirectToAction("LogIn", "Pub");
                }

                if (users.Count() > 1)
                {
                    user.UserName = user.UserName + user.LastName.Substring(0, 1);
                    _context.Update(user);
                    _context.SaveChanges();
                    //BackgroundJob.Enqueue(() => Mailer.ConfirmUser(user.Id));
                }

                if (user.Password == Password._one_way_encrypt(login.Password, user.Id))
                {
                    if (!user.IsActive && user.LastLogon == null)
                    {
                        TempData["Info"] = "Your account is not activated yet. Please contact the system administrator.";
                        return RedirectToAction("Login", "Pub");
                    }
                    if (!user.IsActive)
                    {
                        TempData["Error"] = "Your account has been blocked. Please contact the system administrator.";
                        return RedirectToAction("LogIn", "Pub");
                    }

                    user.FailureCount = 0;
                    user.LastLogon = DateTime.Now;
                    _context.Update(user);
                    _context.SaveChanges();

                   
                    _um.LoadUserRole(user.Id);
                   _um.setUserBasicInfo(user);
                    if (Headedto != null) return Redirect(Headedto);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    if (user.FailureCount == 5)
                    {
                        user.BlockEndDate = DateTime.Now.AddMinutes(15);
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        if (user.FailureCount == 4)
                        {
                            //BackgroundJob.Enqueue(() => Mailer.crackAttempt(user.Id));
                        }
                        user.FailureCount += 1;
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            TempData["Error"] = "Invalid User name or password";
            return RedirectToAction("LogIn", "Pub");
        }


        public IActionResult SignUp()
        {

            return View();

        }

        [HttpPost]

        public IActionResult SignUp(string firstName, string middleName, string phoneNumber, string email, string lastName, int genderId, string userName, string password)
        {
          
                var newUser = new User
                {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    UserName = userName,
                    Password = password,
                    //DefaultLanguageId = 1,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    GenderId = genderId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    LastLogon = DateTime.Now,
                    

                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
                encryptPassword(newUser.Id, newUser.Password);

                TempData["Success"] = " you have successfully registered!!";

                return RedirectToAction("Login");

           
                //TempData["Error"] = "Not registered.";
                //return View();
            
        }

        public IActionResult EquipDetails(int id)
        {
            var equipment = _context.Equipments
                .Include(e => e.EquipmentsType)
                .Include(e => e.Model) 
                .Include(e => e.Manufacturer) 
                .FirstOrDefault(e => e.Id == id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        private void encryptPassword(int userId, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            user.Password = Password._one_way_encrypt(password, userId);
            _context.Update(user);
            _context.SaveChanges();
        }

        public ActionResult Logout()
        {
            TempData["Headedto"] = null;
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //public IActionResult Equipment(string search)
        //{
        //    var equipment = _context.Equipments.AsQueryable();

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        equipment = equipment.Where(e => e.Name.Contains(search));
        //        ViewBag.SearchTerm = search;
        //    }

        //    return View(equipment.ToList());
        //}


    }
}
