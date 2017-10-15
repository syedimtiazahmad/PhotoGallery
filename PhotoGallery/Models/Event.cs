using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace PhotoGallery.Models
{
    public class Event:DbContext
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
    }
}