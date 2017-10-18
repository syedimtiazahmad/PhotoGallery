using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoGallery.Models;
namespace PhotoGallery.ViewModels
{
    public class UserFormViewModel
    {
        public IEnumerable<Role> Roles { get; set; }
        public User User { set; get; }
    }
}