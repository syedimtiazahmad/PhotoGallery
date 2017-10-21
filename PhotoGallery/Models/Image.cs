using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace PhotoGallery.Models
{
    public class Image
    {
        public int Id { set; get; }
        public string Path { set; get; }
        public int EventId { set; get; }
        public string Name { set; get; }
    }
}