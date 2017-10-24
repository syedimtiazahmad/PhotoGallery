using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoGallery.Models;
namespace PhotoGallery.ViewModels
{
    public class UsersFetchViewModel
    {
        public IQueryable<User> Users { set; get; }
    }
}