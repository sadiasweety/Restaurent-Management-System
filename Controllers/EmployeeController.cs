using RestaurentManagementSystem_EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurentManagementSystem_EF.Controllers
{
    public class EmployeeController : Controller
    {
        private Model1 db = new Model1();
        public ActionResult Index()
        {
            List<EmployeeListViewModel> list = db.tblEmployees.Select(t => new EmployeeListViewModel
            {
                EmployeeId = t.EmployeeId,
                EmployeeName = t.EmployeeName,
                Email = t.Email,
                DoB = t.DoB,
                ImageName = t.ImageName,
                ImageUrl = t.ImageUrl
            }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult AddOrEdit(EmployeeCreateViewModel viewObj)
        {
            var result = false;
            string fileName = Path.GetFileNameWithoutExtension(viewObj.ImageFile.FileName);
            string extension = Path.GetExtension(viewObj.ImageFile.FileName);
            string fileWithExtension = fileName + extension;
            tblEmployee trObj = new tblEmployee();
            trObj.EmployeeName = viewObj.EmployeeName;
            trObj.Email = viewObj.Email;
            trObj.DoB = viewObj.DoB;
            trObj.ImageName = fileWithExtension;
            trObj.ImageUrl = "~/Images/" + fileName + extension;
            string serverPath = Path.Combine(Server.MapPath("~/Images/" + fileName + extension));
            viewObj.ImageFile.SaveAs(serverPath);
            if (ModelState.IsValid)
            {
                if (viewObj.EmployeeId == 0)
                {
                    db.tblEmployees.Add(trObj);
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    trObj.EmployeeId = viewObj.EmployeeId;
                    db.Entry(trObj).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }

            }
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (viewObj.EmployeeId == 0)
                {
                    return View("Create");
                }
                else
                {
                    return View("Edit");
                }
            }

        }
        public ActionResult Edit(int id)
        {
            tblEmployee trObj = db.tblEmployees.SingleOrDefault(t => t.EmployeeId == id);
            EmployeeCreateViewModel viewObj = new EmployeeCreateViewModel();
            viewObj.EmployeeId = trObj.EmployeeId;
            viewObj.EmployeeName = trObj.EmployeeName;
            viewObj.Email = trObj.Email;
            viewObj.DoB = trObj.DoB;
            viewObj.ImageUrl = trObj.ImageUrl;
            viewObj.ImageName = trObj.ImageName;
            return View(viewObj);
        }
        public ActionResult Delete(int id)
        {
            tblEmployee trObj = db.tblEmployees.SingleOrDefault(t => t.EmployeeId == id);
            {
                db.tblEmployees.Remove(trObj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
    }
}