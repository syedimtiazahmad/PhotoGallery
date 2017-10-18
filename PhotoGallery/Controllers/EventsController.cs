﻿using System;
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
    public class EventsController : Controller
    {

        private ApplicationDbContext _context;

        public EventsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Events
        public ActionResult Details()
        {
            var user = new  User();
            return View(user);
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            var dbEvent = new Event();
            //if (UserSession() == 0)
            //    return RedirectToAction("Index", "Sessions");
            return View(dbEvent);
        }

        private int UserSession()
        {
            return Convert.ToInt32(Session["user_id"]);
        }
    }
}