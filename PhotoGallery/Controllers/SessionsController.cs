using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoGallery.Models;
using System.Security.Cryptography;

namespace PhotoGallery.Controllers
{
    public class SessionsController : Controller
    {
        private ApplicationDbContext _context;

        public SessionsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [Route("Login")]
        public ActionResult New()
        {
            if (UserSession() != 0)
                return RedirectToAction("Index", "Events");
            ViewBag.Title = "Sign In";
            var user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(User user_params)
        {
            if (user_params == null)
                return RedirectToAction("New");
            var user = _context.User.SingleOrDefault(m=>m.Email == user_params.Email);
            if (user == null)
                return RedirectToAction("New");
            Session["user_id"] = user.Id;
            Session["user_email"] = user.Email;
            Session["current_user_role_id"] = user.RoleId;
            return RedirectToAction("Detail", "Users", new { id=user.Id});
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id != 0 || id != null)
            {
                Session.Remove("user_id");
                Session.Remove("user_email");
                Session.Remove("current_user_role_id");
            }                
            return RedirectToAction("Index", "Events");
        }

        //private static string GenerateHash(string value, string salt)
        //{
        //    byte[] data = System.Text.Encoding.ASCII.GetBytes(salt + value);
        //    data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
        //    return Convert.ToBase64String(data);
        //}

        //public static string CreateSalt()
        //{
        //    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        //    byte[] buff = new byte[32];
        //    rng.GetBytes(buff);
        //    return Convert.ToBase64String(buff);
        //}
        private int UserSession()
        {
            return Convert.ToInt32(Session["user_id"]);
        }
    }
}