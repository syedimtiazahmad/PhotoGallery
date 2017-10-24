using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace PhotoGallery.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public ICollection<Image> Images { set; get; }

        public int UserId { set; get; }

        public string location { set; get; }
        public DateTime? CreatedAt { set; get; }
        public DateTime? UpdatedAt { set; get; }
    }
}