// Controllers/UserController.cs
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class UserController : Controller
    {
        public static System.Collections.Generic.List<User> userlist
            = new System.Collections.Generic.List<User>();

        // GET: User
        // now takes an optional ?searchString=...
        public ActionResult Index(string? searchString = null)
        {
            ViewData["CurrentFilter"] = searchString;

            // start with full list
            var users = userlist.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                // filter by name or email, case-insensitive
                users = users.Where(u =>
                    u.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    || u.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                );
            }

            return View(users.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = userlist.Count > 0 ? userlist.Max(u => u.Id) + 1 : 1;
                userlist.Add(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var existingUser = userlist.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                return NotFound();

            existingUser.Name  = user.Name;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            // etc.

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            userlist.Remove(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
