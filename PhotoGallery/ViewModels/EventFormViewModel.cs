using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoGallery.Models;
namespace PhotoGallery.ViewModels
{
    public class EventFormViewModel
    {
        public Event Event { set; get; }
        public IQueryable<Event> Events { set; get; }
        public IQueryable<Image> Images { set; get; }
        public HttpPostedFileBase[] UploadedImages { set; get; }
    }
}