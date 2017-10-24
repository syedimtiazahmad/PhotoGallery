using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoGallery.Models;
namespace PhotoGallery.ViewModels
{
    public class EventsParametersViewModel
    {
        public int? user_id { set; get; }
        public string search { set; get; }
    }
}