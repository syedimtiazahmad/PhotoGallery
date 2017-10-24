using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoGallery.Models;
using PhotoGallery.ViewModels;
using System.IO;
using System.Collections;
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
        public ActionResult Detail(int id)
        {
            var dbEvent = _context.Event.SingleOrDefault(m => m.Id == id);
            var images = _context.Image.Where(m => m.EventId == id);
            var viewModel = new EventFormViewModel
            {
                Event = dbEvent,
                Images = images
            };
            return View(viewModel);
        }


        public ActionResult Index(EventsParametersViewModel event_params)
        {
            var dbEvents = _context.Event.Include(e => e.Images);
            if (Convert.ToInt32(event_params.user_id) != 0)
                dbEvents = dbEvents.Where(e => e.UserId == event_params.user_id);
            //dbEvents = dbEvents.Where(m=>m.Name.Contains("Hallow"));
            if (event_params != null && event_params.search != null)
                dbEvents = dbEvents.Where(e => e.Name.Contains(event_params.search) || e.location.Contains(event_params.search) );
            var viewModel = new EventFormViewModel
            {
                Events = dbEvents.AsQueryable(),
            };
            return View(viewModel);
        }

        public ActionResult New()
        {
            var dbEvent = new Event();
            var viewModel = new EventFormViewModel
            {
                Event = dbEvent
            };
            //if (UserSession() == 0)
            //    return RedirectToAction("Index", "Sessions");
            return View(viewModel);
        }

        public ActionResult Create(EventFormViewModel eventModel)
        {
            //if (UserSession() == 0)
            //    return RedirectToAction("Index", "Events");
            //_context.Event.Add(eventModel.Event);
            _context.Event.Add(eventModel.Event);
            _context.SaveChanges();
            int id = eventModel.Event.Id;
            //ArrayList _filenames = new ArrayList();
            //ArrayList _filePath = new ArrayList();
            if (eventModel.UploadedImages[0] != null)
            {
                foreach (var img in eventModel.UploadedImages)
                {
                    Image imgObj = new Image();
                    string filename = Path.GetFileNameWithoutExtension(img.FileName);
                    string extension = Path.GetExtension(img.FileName);
                    filename = filename + DateTime.Now.ToString("yymm") + extension;
                    imgObj.Name = filename;
                    string relative_path = "/Images/" + filename;
                    imgObj.Path = relative_path;
                    string filepath = Server.MapPath("~" + relative_path);
                    imgObj.EventId = id;
                    img.SaveAs(filepath);
                    _context.Image.Add(imgObj);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Detail", new { id= id});
        }

        public ActionResult Edit(int id)
        {
            var dbEvent = _context.Event.SingleOrDefault(e => e.Id == id);
            var images = _context.Image.Where(m => m.EventId == id);
            var viewModel = new EventFormViewModel
            {
                Event = dbEvent,
                Images = images
                
            };
            return View(viewModel);
        }

        [HttpPut]
        public ActionResult Update(EventFormViewModel eventModel)
        {
            var dbEvent = _context.Event.SingleOrDefault(m => m.Id == eventModel.Event.Id);
            dbEvent.Name = eventModel.Event.Name;
            dbEvent.Description = eventModel.Event.Description;
            dbEvent.UserId = eventModel.Event.UserId;
            dbEvent.location = eventModel.Event.location;
            if(eventModel.UploadedImages[0] != null)
            {
                var imgs = _context.Image.Where(m => m.EventId == dbEvent.Id);
                foreach (var img in imgs)
                {
                    _context.Image.Remove(img);
                }
                foreach (var img in eventModel.UploadedImages)
                {
                    Image imgObj = new Image();
                    string filename = Path.GetFileNameWithoutExtension(img.FileName);
                    string extension = Path.GetExtension(img.FileName);
                    filename = filename + DateTime.Now.ToString("yymm") + extension;
                    imgObj.Name = filename;
                    string relative_path = "/Images/" + filename;
                    imgObj.Path = relative_path;
                    string filepath = Server.MapPath("~" + relative_path);
                    imgObj.EventId = dbEvent.Id;
                    img.SaveAs(filepath);
                    _context.Image.Add(imgObj);
                    _context.SaveChanges();
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Detail", new { id = eventModel.Event.Id });
        }
        private int UserSession()
        {
            return Convert.ToInt32(Session["user_id"]);
        }
    }
}