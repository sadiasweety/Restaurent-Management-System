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
    public class CustomerController : Controller
    {
        // GET: Customer
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll()
        {
            return View(GetAllEmployee());
        }
        IEnumerable<CustomerInfo> GetAllEmployee()
        {
            using (Model1 db = new Model1())
            {
                return db.CustomerInfos.ToList<CustomerInfo>();
            }

        }
        public ActionResult AddOrEdit(int id = 0)
        {
            CustomerInfo cus = new CustomerInfo();
            if (id != 0)
            {
                using (Model1 db = new Model1())
                {
                    cus = db.CustomerInfos.Where(x => x.CustomerID == id).FirstOrDefault<CustomerInfo>();
                }
            }
            return View(cus);
        }
        [HttpPost]
        public ActionResult AddOrEdit(CustomerInfo cus)
        {
            try
            {
                if (cus.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(cus.ImageUpload.FileName);
                    string extension = Path.GetExtension(cus.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    cus.ImagePath = "~/AppFiles/Images/" + fileName;
                    cus.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (Model1 db = new Model1())
                {
                    if (cus.CustomerID == 0)
                    {
                        db.CustomerInfos.Add(cus);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(cus).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                return RedirectToAction("ViewAll");
                //return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                using (Model1 db = new Model1())
                {
                    CustomerInfo emp = db.CustomerInfos.Where(x => x.CustomerID == id).FirstOrDefault<CustomerInfo>();
                    db.CustomerInfos.Remove(emp);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}