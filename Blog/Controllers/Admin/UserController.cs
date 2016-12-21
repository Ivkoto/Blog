using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers.Admin
{
    public class UserController : Controller
    {
        
        // GET: User/List
        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var users = database.Users.ToList();

                return View(users);
            }
        }

        //GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
    }
}