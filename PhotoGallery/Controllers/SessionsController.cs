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
            //var password_salt = CreateSalt();
            //var hash = GenerateHash(user_params.Password, password_salt);
            //return Content(hash);
            Session["user_id"] = user.Id;
            return RedirectToAction("Detail", "Users", new { id=user.Id});
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
    }
}