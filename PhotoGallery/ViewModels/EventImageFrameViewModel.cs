using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoGallery.Models;
namespace PhotoGallery.ViewModels
{
    public class EventImageFrameViewModel
    {
        public Event Event { set; get; }
        public Image Image { set; get; }
        public IQueryable<Frame> Frames { set; get; }
    }
}