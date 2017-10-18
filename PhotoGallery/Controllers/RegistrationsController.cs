using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoGallery.Models;

namespace PhotoGallery.Controllers
{
    public class RegistrationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SignUp
        [Route("SignUp")]
        public ActionResult New()
        {
            if (UserSession() != 0)
                return RedirectToAction("Index", "Events");
            var user = new User();
            return View(user);
        }

        // POST: Registrations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user_params)
        {
            if (!ModelState.IsValid) {
                var user = new User();
                return View("New");
            }
            db.User.Add(user_params);
            db.SaveChanges();
            return RedirectToAction("New", "Sessions");

        }

        private int UserSession()
        {
            return Convert.ToInt32(Session["user_id"]);
        }
    }
}
