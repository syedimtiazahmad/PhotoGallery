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
            var user = new User();
            return View(user);
        }
      
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        // GET: Registrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
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
    }
}
