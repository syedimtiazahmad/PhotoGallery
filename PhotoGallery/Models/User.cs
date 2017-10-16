using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PhotoGallery.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int age { set; get; }

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }
    }
}