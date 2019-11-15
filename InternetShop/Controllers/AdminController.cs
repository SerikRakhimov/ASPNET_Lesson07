using InternetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetShop.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Users()
        {
            using (var context = new Context())
            {
                ViewBag.Users = context.Users.ToList();
            }

            return View();
        }
        public new ActionResult User(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                var user = context.Users.FirstOrDefault(p => p.Id == id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                ViewBag.User = user;
            }

            return View();
        }

        public ActionResult AddUser()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(User user)
        {
            if (user == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "";
                return View(user);
            }

            using (var context = new Context())
            {
                var resultSeek = context.Users.Any(x => x.Login == user.Login);
                if (resultSeek)
                {
                    ViewBag.Message = "Логин \"" + user.Login + "\" уже есть в базе данных. Повторите ввод!";
                    return View(user);
                }
                context.Users.Add(user);
                context.SaveChanges();
            }
            return RedirectToAction("Users");

        }

        public ActionResult EditUser()
        {
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditUser(int? id)
        {
            User user = null;

            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                user = context.Users.FirstOrDefault(p => p.Id == id);
                if (user == null)
                {
                    return HttpNotFound();
                }
            }
            ViewBag.Message = "";
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            User us;
            if (user == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "";
                return View(user);
            }

            using (var context = new Context())
            {

                us = context.Users.Find(user.Id);
                if (us != null)
                {
                    if (us.Login != user.Login)
                    {

                        var resultSeek = context.Users.Any(x => x.Login == user.Login);
                        if (resultSeek)
                        {
                            ViewBag.Message = "Логин \"" + user.Login + "\" уже есть в базе данных. Повторите ввод!";
                            return View(user);
                        }
                    }
                    us.Login = user.Login;
                    us.Password = user.Password;
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Users");
        }

        public ActionResult DeleteUser()
        {
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteUser(int? id)
        {
            User user = null;

            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                user = context.Users.FirstOrDefault(p => p.Id == id);
                if (user == null)
                {
                    return HttpNotFound();
                }

            }
            return View(user);
        }

        [HttpPost]
        public ActionResult DeleteUser(User user)
        {
            User us;
            if (user == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            using (var context = new Context())
            {

                us = context.Users.Find(user.Id);
                if (us != null)
                {
                    context.Users.Remove(us);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Users");
        }

    }
}