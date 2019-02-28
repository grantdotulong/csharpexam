using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using beltExamTwo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace beltExamTwo.Controllers
{
    public class HomeController : Controller
    {
        private beltexamtwoContext dbContext;
        public HomeController(beltexamtwoContext context)
        {
            dbContext = context;
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpGet("")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u=>u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("logged-in", 1);
                    HttpContext.Session.SetInt32("userId", newUser.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            return View("Index");
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u=>u.Email == userSubmission.LoginEmail);
                if(userInDb is null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password.");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password.");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("logged-in", 1);
                HttpContext.Session.SetInt32("userId", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpGet("Hobby")]
        public IActionResult Dashboard()
        {
            Dashboard dash = new Dashboard();
            List<Hobby> allHobbies = dbContext.Hobbies
                .Include(h => h.Users)
                .OrderByDescending(h => h.Users.Count)
                .ToList();
            User currUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
            List<Hobby> novice = dbContext.Hobbies
                .Include(a => a.Users)
                .Where(p => p.Users.Any(a => a.Proficiency == "Novice"))
                .OrderByDescending(h => h.Users.Count)
                .ToList();
            List<Hobby> intermediate = dbContext.Hobbies
                .Include(a => a.Users)
                .Where(p => p.Users.Any(a => a.Proficiency == "Intermediate"))
                .OrderByDescending(h => h.Users.Count)
                .ToList();
            List<Hobby> expert = dbContext.Hobbies
                .Include(a => a.Users)
                .Where(p => p.Users.Any(a => a.Proficiency == "Expert"))
                .OrderByDescending(h => h.Users.Count)
                .ToList();
            dash.ActiveUser = currUser;
            dash.AllHobbies = allHobbies;
            dash.Novice = novice;
            dash.Intermediate = intermediate;
            dash.Expert = expert;
            return View(dash);
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpGet("Hobby/New")]
        public IActionResult DisplayCreateHobby()
        {
            ViewBag.ActiveUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
            return View("newHobby_display");
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpPost("Hobby/New")]
        public IActionResult CreateHobby(Hobby newHobby)
        {
            if(ModelState.IsValid)
            {
                dbContext.Hobbies.Add(newHobby);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                User currUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
                ViewBag.ActiveUser = currUser;
                return View("newHobby_display");
            }
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpGet("Hobby/{hobbyId}")]
        public IActionResult DisplayHobbyInfo(int hobbyId)
        {
            HobbyInfo info = new HobbyInfo();
            Hobby selectedHobby = dbContext.Hobbies
                .Include(h => h.CreatedBy)
                .Include(h => h.Users)
                    .ThenInclude(u => u.User)
                .FirstOrDefault(h => h.HobbyId == hobbyId);
            User currUser = dbContext.Users
                .Include(u => u.Hobbies)
                    .ThenInclude(h => h.Hobby)
                .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
            info.currentHobby = selectedHobby;
            info.currentUser = currUser;
            HttpContext.Session.SetInt32("hobbyId", (int)hobbyId);
            ViewBag.ActiveUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
            ViewBag.CurrentHobby = dbContext.Hobbies.FirstOrDefault(a => a.HobbyId == hobbyId);
            return View("hobbyinfo_display", info);
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpPost("New-Association")]
        public IActionResult NewAsc(Association newAsc)
        {
            if(ModelState.IsValid)
            {
                dbContext.Associations.Add(newAsc);
                dbContext.SaveChanges();

                HobbyInfo info = new HobbyInfo();
                Hobby selectedHobby = dbContext.Hobbies
                .Include(h => h.CreatedBy)
                .Include(h => h.Users)
                    .ThenInclude(u => u.User)
                .FirstOrDefault(h => h.HobbyId == HttpContext.Session.GetInt32("hobbyId"));
                User currUser = dbContext.Users
                .Include(u => u.Hobbies)
                    .ThenInclude(h => h.Hobby)
                .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
                info.currentHobby = selectedHobby;
                info.currentUser = currUser;
                ViewBag.ActiveUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
                ViewBag.CurrentHobby = dbContext.Hobbies.FirstOrDefault(a => a.HobbyId == HttpContext.Session.GetInt32("hobbyId"));
                return View("hobbyinfo_display", info);
            }
                HobbyInfo info2 = new HobbyInfo();
                Hobby selectedHobby2 = dbContext.Hobbies
                .Include(h => h.CreatedBy)
                .Include(h => h.Users)
                    .ThenInclude(u => u.User)
                .FirstOrDefault(h => h.HobbyId == HttpContext.Session.GetInt32("hobbyId"));
                User currUser2 = dbContext.Users
                .Include(u => u.Hobbies)
                    .ThenInclude(h => h.Hobby)
                .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
                info2.currentHobby = selectedHobby2;
                info2.currentUser = currUser2;
                ViewBag.ActiveUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
                ViewBag.CurrentHobby = dbContext.Hobbies.FirstOrDefault(a => a.HobbyId == HttpContext.Session.GetInt32("hobbyId"));
                return View("hobbyinfo_display", info2);
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpGet("Hobby/Edit/{hobbyId}")]
        public IActionResult DisplayHobbyEdit(int hobbyId)
        {
            Hobby selectedHobby = dbContext.Hobbies.FirstOrDefault(h => h.HobbyId == hobbyId);
            ViewBag.ActiveUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
            return View("hobbyedit_display", selectedHobby);
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
        [HttpPost("Hobby-Edit")]
        public IActionResult EditHobby(Hobby updatedHobby)
        {
            if(ModelState.IsValid)
            {
                Hobby toUpdate = dbContext.Hobbies.FirstOrDefault(a => a.HobbyId == HttpContext.Session.GetInt32("hobbyId"));
                toUpdate.HobbyName = updatedHobby.HobbyName;
                toUpdate.Description = updatedHobby.Description;
                dbContext.SaveChanges();


                HobbyInfo info = new HobbyInfo();
                User currUser = dbContext.Users
                .Include(u => u.Hobbies)
                    .ThenInclude(h => h.Hobby)
                .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
                info.currentHobby = toUpdate;
                info.currentUser = currUser;
                ViewBag.ActiveUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
                ViewBag.CurrentHobby = dbContext.Hobbies.FirstOrDefault(a => a.HobbyId == HttpContext.Session.GetInt32("hobbyId"));
                return View("hobbyinfo_display", info);
            }
            Hobby selectedHobby = dbContext.Hobbies.FirstOrDefault(h => h.HobbyId ==  HttpContext.Session.GetInt32("hobbyId"));
            ViewBag.ActiveUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
            return View("hobbyedit_display", selectedHobby);
        }
//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+

//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+

//+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
    }
}
