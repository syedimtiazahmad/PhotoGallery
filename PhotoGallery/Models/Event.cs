using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
namespace PhotoGallery.Models
{
    public class Event
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { set; get; }

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Minimum 10 characters required")]
        public string Description { set; get; }

        public ICollection<Image> Images { set; get; }

        public int UserId { set; get; }
        
        public string location { set; get; }
        public DateTime? CreatedAt { set; get; }
        public DateTime? UpdatedAt { set; get; }
    }
}