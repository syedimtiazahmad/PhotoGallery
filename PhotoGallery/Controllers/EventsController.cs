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
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
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
            int user_id = UserSession();
            var current_user = _context.User.SingleOrDefault(u => u.Id == user_id);
            var viewModel = new EventFormViewModel
            {
                Event = dbEvent,
                Images = images,
                User = current_user
            };
            return View(viewModel);
        }


        public ActionResult Index(EventsParametersViewModel event_params)
        {
            var dbEvents = _context.Event.Include(e => e.Images);
            if (event_params.user_id != null && Convert.ToInt32(event_params.user_id) != 0)
                dbEvents = dbEvents.Where(e => e.UserId == event_params.user_id);
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
            if (UserSession() == 0)
                return RedirectToAction("Index", "Sessions");
            return View(viewModel);
        }

        public ActionResult Create(EventFormViewModel eventModel)
        {
            if (UserSession() == 0)
                return RedirectToAction("Index", "Events");
            if (!ModelState.IsValid)
            {
                var dbEvent = new Event();
                var viewModel = new EventFormViewModel
                {
                    Event = dbEvent
                };
                return View("New", viewModel);
            }
            _context.Event.Add(eventModel.Event);
            _context.SaveChanges();
            int id = eventModel.Event.Id;

            if (eventModel.UploadedImages[0] != null)
            {
                foreach (var img in eventModel.UploadedImages)
                {
                    PhotoGallery.Models.Image imgObj = new PhotoGallery.Models.Image();
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
            if (UserSession() == 0)
                return RedirectToAction("Index", "Sessions");
            var dbEvent = _context.Event.SingleOrDefault(m => m.Id == eventModel.Event.Id);

            if (!ModelState.IsValid)
            {
                var viewModel = new EventFormViewModel
                {
                    Event = dbEvent
                };
                return View("Edit", viewModel);
            }
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
                    PhotoGallery.Models.Image imgObj = new PhotoGallery.Models.Image();
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

        [HttpGet]
        [Route("Events/Delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            if (UserSession() == 0)
                return RedirectToAction("New", "Sessions");
            var current_event = _context.Event.SingleOrDefault(u => u.Id == id);
            var current_event_images = _context.Image.Where(m => m.EventId == current_event.Id);
            foreach (PhotoGallery.Models.Image img in current_event_images)
            {
                _context.Image.Remove(img);
            }
            _context.Event.Remove(current_event);
            _context.SaveChanges();
            return RedirectToAction("Index", "Events");
        }

        [Route("Events/Images/{id:int}/AddFrame")]
        public ActionResult AddFrame(int id)
        {
            var img = _context.Image.SingleOrDefault(i => i.Id == id);
            var frames = _context.Frame;
            var viewModel = new EventImageFrameViewModel
            {
                Image = img,
                Frames = frames
            };
            return View(viewModel);
        }

        [Route("Images/CreateFrame")]
        [HttpPost]
        public ActionResult CreateFrame(EventImageFrameViewModel viewModel)
        {
            var img = _context.Image.SingleOrDefault(m => m.Id == viewModel.Image.Id);
            img.FrameId = viewModel.Image.FrameId;
            var frame = _context.Frame.SingleOrDefault(m => m.Id == viewModel.Image.FrameId);
            AddFrame(frame, img);
            _context.SaveChanges();
            return RedirectToAction("Detail", new { id = img.EventId});
        }


        // ===========  PRIVATE METHOD ================
        private int UserSession()
        {
            return Convert.ToInt32(Session["user_id"]);
        }

        private void AddFrame(Frame frame, PhotoGallery.Models.Image img)
        {
            switch (frame.Code)
            {
                case 1:
                    SinglePurpleFrame(img);
                    break;
                case 2:
                    SingleBurlyWoodFrame(img);
                    break;
                case 3:
                    DoubleColorsFrame(img);
                    break;
                case 4:
                    TripleColorsFrame(img);
                    break;
            }
        }

        private void SinglePurpleFrame(PhotoGallery.Models.Image img)
        {
            //Response.Clear();
            Bitmap bmp = new Bitmap(Server.MapPath(img.Path));
            int height = bmp.Height;
            int width = bmp.Width;

            Bitmap bmp1 = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(bmp);
            Graphics g1 = Graphics.FromImage(bmp1);

            g1.DrawImage(bmp,
            new Rectangle(0, 0, width, height),
            new Rectangle(0, 0, width, height),
            GraphicsUnit.Pixel);

            g1.TextRenderingHint = TextRenderingHint.AntiAlias;
            Pen purplePen = new Pen(Brushes.Purple);

            purplePen.Width = 24.0F;
            g1.DrawRectangle(purplePen, 0, 0, width, height);

            string _imagepath = Server.MapPath(img.Path);

            g.Dispose();
            bmp.Dispose();
            System.IO.File.Delete(_imagepath);
            bmp1.Save(_imagepath, ImageFormat.Jpeg);

            g1.Dispose();

            bmp1.Dispose();
            //Response.End();
        }

        private void SingleBurlyWoodFrame(PhotoGallery.Models.Image img)
        {

            //Response.Clear();
            Bitmap bmp = new Bitmap(Server.MapPath(img.Path));
            int height = bmp.Height;
            int width = bmp.Width;

            Bitmap bmp1 = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(bmp);
            Graphics g1 = Graphics.FromImage(bmp1);

            g1.DrawImage(bmp,
            new Rectangle(0, 0, width, height),
            new Rectangle(0, 0, width, height),
            GraphicsUnit.Pixel);

            g1.TextRenderingHint = TextRenderingHint.AntiAlias;
            Pen burlyWoodPen = new Pen(Brushes.BurlyWood);

            burlyWoodPen.Width = 24.0F;
            g1.DrawRectangle(burlyWoodPen, 0, 0, width, height);

            string _imagepath = Server.MapPath(img.Path);

            g.Dispose();
            bmp.Dispose();
            System.IO.File.Delete(_imagepath);
            bmp1.Save(_imagepath, ImageFormat.Jpeg);

            g1.Dispose();

            bmp1.Dispose();
            //Response.End();
        }

        private void DoubleColorsFrame(PhotoGallery.Models.Image img)
        {
            //Response.Clear();

            Bitmap bmp = new Bitmap(Server.MapPath(img.Path));
            int height = bmp.Height;
            int width = bmp.Width;

            Bitmap bmp1 = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            //Graphics g = Graphics.FromImage(bmp);
            Graphics g1 = Graphics.FromImage(bmp1);

            g1.DrawImage(bmp,
            new Rectangle(0, 0, width, height),
            new Rectangle(0, 0, width, height),
            GraphicsUnit.Pixel);

            g1.TextRenderingHint = TextRenderingHint.AntiAlias;
            Pen cadetBluePen = new Pen(Brushes.CadetBlue);
            Pen darkGreenPen = new Pen(Brushes.DarkGreen);
            cadetBluePen.Width = 12.0F;
            darkGreenPen.Width = 12.0F;

            g1.DrawRectangle(cadetBluePen, 5, 5, width - 12, height - 12);
            g1.DrawRectangle(darkGreenPen, 0, 0, width, height);

            string _imagepath = Server.MapPath(img.Path);

            //g.Dispose();
            bmp.Dispose();
            System.IO.File.Delete(_imagepath);
            bmp1.Save(_imagepath, ImageFormat.Jpeg);

            g1.Dispose();

            bmp1.Dispose();
            //Response.End();
        }

        private void TripleColorsFrame(PhotoGallery.Models.Image img)
        {
            //Response.Clear();

            Bitmap bmp = new Bitmap(Server.MapPath(img.Path));
            int height = bmp.Height;
            int width = bmp.Width;

            Bitmap bmp1 = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            //Graphics g = Graphics.FromImage(bmp);
            Graphics g1 = Graphics.FromImage(bmp1);

            g1.DrawImage(bmp,
            new Rectangle(0, 0, width, height),
            new Rectangle(0, 0, width, height),
            GraphicsUnit.Pixel);

            g1.TextRenderingHint = TextRenderingHint.AntiAlias;
            Pen whitePen = new Pen(Brushes.White);
            Pen orangePen = new Pen(Brushes.Orange);
            Pen redPen = new Pen(Brushes.Red);
            whitePen.Width = 8.0F;
            orangePen.Width = 8.0F;
            redPen.Width = 8.0F;

            g1.DrawRectangle(whitePen, 10, 10, width - 20, height - 20);
            g1.DrawRectangle(orangePen, 5, 5, width - 12, height - 12);
            g1.DrawRectangle(redPen, 0, 0, width, height);

            string _imagepath = Server.MapPath(img.Path);

            //g.Dispose();
            bmp.Dispose();
            System.IO.File.Delete(_imagepath);
            bmp1.Save(_imagepath, ImageFormat.Jpeg);

            g1.Dispose();

            bmp1.Dispose();
            //Response.End();
        }
    }
}