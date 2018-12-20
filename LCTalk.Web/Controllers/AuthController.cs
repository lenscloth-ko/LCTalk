using LCTalk.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCTalk.Web.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public ActionResult Login()
        {
            string user_name = Request.Form["username"];

            if (user_name.Trim() == "")
            {
                return Redirect("/");
            }

            using (var db = new Models.ChatContext())
            {

                User user = db.Users.FirstOrDefault(u => u.name == user_name);

                if (user == null)
                {
                    user = new User { name = user_name };

                    db.Users.Add(user);
                    db.SaveChanges();
                }

                Session["user"] = user;
            }

            return Redirect("/chat");
        }
    }
}