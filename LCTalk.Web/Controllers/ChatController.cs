using LCTalk.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCTalk.Web.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return Redirect("/");
            }

            var currentUser = (Models.User)Session["user"];

            using (var db = new Models.ChatContext())
            {

                ViewBag.allUsers = db.Users.Where(u => u.name != currentUser.name)
                                 .ToList();
            }


            ViewBag.currentUser = currentUser;


            return View();
        }

        public JsonResult ConversationWithContact(int contact)
        {
            if (Session["user"] == null)
            {
                return Json(new { status = "error", message = "User is not logged in" });
            }

            var currentUser = (Models.User)Session["user"];

            var conversations = new List<Models.Conversation>();

            using (var db = new Models.ChatContext())
            {
                conversations = db.Conversations.
                                  Where(c => (c.receiver_id == currentUser.id
                                      && c.sender_id == contact) ||
                                      (c.receiver_id == contact
                                      && c.sender_id == currentUser.id))
                                  .OrderBy(c => c.created_at)
                                  .ToList();
            }

            return Json(
                new { status = "success", data = conversations },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult SendMessage()
        {
            if (Session["user"] == null)
            {
                return Json(new { status = "error", message = "User is not logged in" });
            }

            var currentUser = (User)Session["user"];

            string socket_id = Request.Form["socket_id"];

            Conversation convo = new Conversation
            {
                sender_id = currentUser.id,
                message = Request.Form["message"],
                receiver_id = Convert.ToInt32(Request.Form["contact"])
            };

            using (var db = new Models.ChatContext())
            {
                db.Conversations.Add(convo);
                db.SaveChanges();
            }

            return Json(convo);
        }
    }
}