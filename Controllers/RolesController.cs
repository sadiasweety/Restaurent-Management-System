using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RestaurentManagementSystem_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurentManagementSystem_EF.Controllers
{
    public class RolesController : Controller
    {
             // GET: Roles
            [Authorize(Roles = "Admin")]
            public ActionResult Index()
            {
                // Populate DropdownList
                var context = new ApplicationDbContext();

                var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                  new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = roleList;

                var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                  new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                ViewBag.Users = userList;



                return View();
            }


        [Authorize(Roles = "Admin")]

        public ActionResult GetRoles()
            {
                // Populate DropdownList
                var context = new ApplicationDbContext();

                var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                  new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = roleList;

                var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                  new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                ViewBag.Users = userList;



                return View();
            }


            [HttpPost]
            public ActionResult GetRoles(string userName)
            {
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    var context = new ApplicationDbContext();
                    ApplicationUser user = context.Users.Where(u => u.UserName.Equals(userName,
                    StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);

                    // Populate DropdownList
                    var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                      new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                    ViewBag.Roles = roleList;

                    var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                      new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                    ViewBag.Users = userList;

                    ViewBag.Message = "Role Retrieved Successfully!!!";
                }
                return View("Index");
            }

        [Authorize(Roles = "Admin")]

        public ActionResult Create()
            {
                return View();
            }

            [HttpPost]

            public ActionResult Create(FormCollection collection)
            {
                try
                {
                    var context = new ApplicationDbContext();
                    context.Roles.Add(new IdentityRole()
                    {
                        Name = collection["RoleName"]
                    });
                    context.SaveChanges();
                    ViewBag.Message = "Role Created Successfully!!!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            [Authorize(Roles = "Admin")]


            public ActionResult Delete(string roleName)
            {
                var context = new ApplicationDbContext();
                var thisRole = context.Roles.Where(r => r.Name.Equals(roleName,
                    StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                context.Roles.Remove(thisRole);
                context.SaveChanges();
                return RedirectToAction("Index");
            }




            [Authorize(Roles = "Admin")]

            public ActionResult RoleAddToUser()
            {
                var context = new ApplicationDbContext();

                var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                  new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = roleList;

                var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                  new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                ViewBag.Users = userList;



                return View();
            }




            [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
            public ActionResult RoleAddToUser(string userName, string roleName)
            {
                var context = new ApplicationDbContext();
                if (context == null)
                {
                    throw new ArgumentNullException("context", "Context must not be null!!!");
                }
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(userName,
                    StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                userManager.AddToRole(user.Id, roleName);




                return RedirectToAction("Index");
            }

            [Authorize(Roles = "Admin")]

            public ActionResult DeleteRoleForUser()
            {
                // Populate DropdownList
                var context = new ApplicationDbContext();

                var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                  new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = roleList;

                var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                  new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                ViewBag.Users = userList;



                return View();
            }

            [HttpPost]


            public ActionResult DeleteRoleForUser(string userName, string roleName)
            {
                var account = new AccountController();
                var context = new ApplicationDbContext();
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(userName,
                StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                if (userManager.IsInRole(user.Id, roleName))
                {
                    userManager.RemoveFromRole(user.Id, roleName);
                    ViewBag.Message = "Role removed from this user Successfully!!!!";
                }
                else
                {
                    ViewBag.Message = "This user doesn't belong to selected role!!!!";
                }

                // Populate DropdownList
                var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                  new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = roleList;

                var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                  new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                ViewBag.Users = userList;


                return RedirectToAction("Index");
            }

        }
    }