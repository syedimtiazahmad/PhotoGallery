using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using PhotoGallery.Models;
using PhotoGallery.ViewModels;
namespace PhotoGallery.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext _context;

        public UsersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Users
        public ActionResult Index()
        {
            if (UserSession() == 0)
                return RedirectToAction("New", "Sessions");
            var users =  _context.User.ToList();
            var viewModel = new UsersFetchViewModel { Users = users.AsQueryable() };
            return View(viewModel);
        }

        public ActionResult Detail(int id)
        {
            if (UserSession() == 0)
                return RedirectToAction("New", "Sessions");
            var user = _context.User.SingleOrDefault(m => m.Id == id);
            return View(user);
        }

        public ActionResult Edit(int id)
        {
            if (UserSession() == 0)
                return RedirectToAction("New", "Sessions");
            var user = _context.User.SingleOrDefault(m => m.Id == id);
            var user_id = UserSession();
            var current_user = _context.User.SingleOrDefault(m => m.Id == user_id);
            var viewModel = new UserFormViewModel
            {
                Roles = _context.Role.ToList(),
                User  = user,
                CurrentUser = current_user
            };
            return View(viewModel);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserFormViewModel user_params)
        {
            if (UserSession() == 0)
                return RedirectToAction("New", "Sessions");
            var user = _context.User.SingleOrDefault(u => u.Id == user_params.User.Id);
            if (!ModelState.IsValid)
                return View("Edit", user);
            user.FirstName = user_params.User.FirstName;
            user.LastName  = user_params.User.LastName;
            user.Email = user_params.User.Email;
            user.age = user_params.User.age;
            user.Role = _context.Role.SingleOrDefault(m => m.Id == user_params.User.RoleId);
            user.RoleId = user_params.User.RoleId;
            user.Permission = user_params.User.Permission;
            _context.SaveChanges();
            return RedirectToAction("Detail", "Users", new { id=user.Id} );
        }

        [HttpGet]
        public  ActionResult Delete(int id)
        {
            if (UserSession() == 0)
                return RedirectToAction("New", "Sessions");
            var user = _context.User.SingleOrDefault(u => u.Id == id);
            _context.User.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "Users");
        }

        private int UserSession() {
            return Convert.ToInt32(Session["user_id"]);
        }
    }
}