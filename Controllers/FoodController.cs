using RestaurentManagementSystem_EF.Models;
using RestaurentManagementSystem_EF.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurentManagementSystem_EF.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        public ActionResult Index(int? id)
        {
            var ctx = new Model1();

            var menuWisefoodQty = from p in ctx.foodItems
                                         group p by p.MenuId into g
                                         select new
                                         {
                                             g.FirstOrDefault().MenuId,
                                             Qty = g.Sum(s => s.Quantity)
                                         };
            var listMenu = (from c in ctx.Menus
                                join cwpq in menuWisefoodQty on c.MenuId equals cwpq.MenuId
                                select new VmMenu
                                {
                                    MenuName = c.MenuName,
                                    MenuId = cwpq.MenuId,
                                    Quantity = cwpq.Qty
                                }).ToList();
            var listFood = (from p in ctx.foodItems
                               join c in ctx.Menus on p.MenuId equals c.MenuId
                               where p.MenuId == id
                               select new VmFood
                               {
                                   MenuId = p.MenuId,
                                   MenuName = c.MenuName,
                                   ExpireDate = p.ExpireDate,
                                   ImagePath = p.ImagePath,
                                   Price = p.Price,
                                   ItemId = p.ItemId,
                                   FoodName = p.FoodName,
                                   Quantity = p.Quantity
                               }).ToList();

            var oMenuWiseFood = new VmMenuWiseFood();
            oMenuWiseFood.MenuList = listMenu;
            oMenuWiseFood.FoodList = listFood;
            oMenuWiseFood.MenuId = listFood.Count > 0 ? listFood[0].MenuId : 0;
            oMenuWiseFood.MenuName = listFood.Count > 0 ? listFood[0].MenuName : "";

            return View(oMenuWiseFood);
        }

        public ActionResult Create()
        {
            var model = new VmFoodMenu();
            var ctx = new Model1();
            model.MenuList = ctx.Menus.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Menu model, string[] FoodName, decimal[] Price, int[] Quantity, DateTime[] ExpireDate, HttpPostedFileBase[] imgFile)
        {
            var ctx = new Model1();
            var oMenu = (from c in ctx.Menus where c.MenuName == model.MenuName.Trim() select c).FirstOrDefault();
            if (oMenu == null)
            {
                ctx.Menus.Add(model);
                ctx.SaveChanges();
            }
            else
            {
                model.MenuId = oMenu.MenuId;
            }

            var listFood = new List<FoodItem>();
            for (int i = 0; i < FoodName.Length; i++)
            {
                string imgPath = "";
                if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imgFile[i].FileName);
                    string fileLocation = Path.Combine(
                        Server.MapPath("~/uploads"), fileName);
                    imgFile[i].SaveAs(fileLocation);

                    imgPath = "/uploads/" + imgFile[i].FileName;
                }

                var newFood = new FoodItem();
                newFood.FoodName = FoodName[i];
                newFood.Quantity = Quantity[i];
                newFood.Price = Price[i];
                newFood.ExpireDate = ExpireDate[i];
                newFood.ImagePath = imgPath;
                newFood.Quantity = Quantity[i];
                newFood.MenuId = model.MenuId;
                listFood.Add(newFood);
            }
            ctx.foodItems.AddRange(listFood);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var ctx = new Model1();
            var oFood = (from p in ctx.foodItems
                            join c in ctx.Menus on p.MenuId equals c.MenuId
                            where p.ItemId == id
                            select new VmFood
                            {
                                MenuId = p.MenuId,
                                MenuName = c.MenuName,
                                ExpireDate = p.ExpireDate,
                                ImagePath = p.ImagePath,
                                Price = p.Price,
                                ItemId = p.ItemId,
                                FoodName = p.FoodName,
                                Quantity = p.Quantity
                            }).FirstOrDefault();
            oFood.MenuList = ctx.Menus.ToList(); // for showing category list in view
            return View(oFood);
        }

        [HttpPost]
        public ActionResult Edit(VmFood model)
        {
            var ctx = new Model1();

            string imgPath = "";
            if (model.ImgFile != null && model.ImgFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.ImgFile.FileName);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);
                model.ImgFile.SaveAs(fileLocation);

                imgPath = "/uploads/" + model.ImgFile.FileName;
            }

            var oFood = ctx.foodItems.Where(w => w.ItemId == model.ItemId).FirstOrDefault();
            if (oFood != null)
            {
                oFood.FoodName = model.FoodName;
                oFood.Quantity = model.Quantity;
                oFood.Price = model.Price;
                oFood.ExpireDate = model.ExpireDate;
                oFood.MenuId = model.MenuId;
                if (!string.IsNullOrEmpty(imgPath))
                {
                    var fileName = Path.GetFileName(oFood.ImagePath);
                    string fileLocation = Path.Combine(Server.MapPath("~/uploads"), fileName);
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                }
                oFood.ImagePath = imgPath == "" ? oFood.ImagePath : imgPath;

                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult EditMultiple(int id)
        {
            var ctx = new Model1();
            var oMenuWiseFood = new VmMenuWiseFood();
            var listFood = (from p in ctx.foodItems
                               join c in ctx.Menus on p.MenuId equals c.MenuId
                               where p.MenuId == id
                               select new VmFood
                               {
                                   MenuId = p.MenuId,
                                   MenuName = c.MenuName,
                                   ExpireDate = p.ExpireDate,
                                   ImagePath = p.ImagePath,
                                   Price = p.Price,
                                   ItemId = p.ItemId,
                                   FoodName = p.FoodName,
                                   Quantity = p.Quantity
                               }).ToList();
            oMenuWiseFood.FoodList = listFood;
            // for showing category list in view
            oMenuWiseFood.MenuList = (from c in ctx.Menus
                                                 select new VmMenu
                                                 {
                                                     MenuId = c.MenuId,
                                                     MenuName = c.MenuName
                                                 }).ToList();
            oMenuWiseFood.MenuId = listFood.Count > 0 ? listFood[0].MenuId : 0;
            oMenuWiseFood.MenuName = listFood.Count > 0 ? listFood[0].MenuName : "";
            return View(oMenuWiseFood);
        }

        [HttpPost]
        public ActionResult EditMultiple(Menu model, int[] ItemId, string[] FoodName, decimal[] Price, int[] Quantity, DateTime[] ExpireDate, HttpPostedFileBase[] imgFile)
        {
            var ctx = new Model1();
            var listFood = new List<FoodItem>();
            for (int i = 0; i < FoodName.Length; i++)
            {
                if (ItemId[i] > 0)
                {
                    string imgPath = "";
                    if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(imgFile[i].FileName);
                        string fileLocation = Path.Combine(
                            Server.MapPath("~/uploads"), fileName);
                        imgFile[i].SaveAs(fileLocation);

                        imgPath = "/uploads/" + imgFile[i].FileName;
                    }
                    int pid = ItemId[i];
                    var oFood = ctx.foodItems.Where(w => w.ItemId == pid).FirstOrDefault();
                    if (oFood != null)
                    {
                        oFood.FoodName = FoodName[i];
                        oFood.Quantity = Quantity[i];
                        oFood.Price = Price[i];
                        oFood.ExpireDate = ExpireDate[i];
                        oFood.MenuId = model.MenuId;
                        if (!string.IsNullOrEmpty(imgPath))
                        {
                            var fileName = Path.GetFileName(oFood.ImagePath);
                            string fileLocation = Path.Combine(Server.MapPath("~/uploads"), fileName);
                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }
                        }
                        oFood.ImagePath = imgPath == "" ? oFood.ImagePath : imgPath;
                        ctx.SaveChanges();
                    }
                }
                else if (!string.IsNullOrEmpty(FoodName[i]))
                {
                    string imgPath = "";
                    if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(imgFile[i].FileName);
                        string fileLocation = Path.Combine(
                            Server.MapPath("~/uploads"), fileName);
                        imgFile[i].SaveAs(fileLocation);

                        imgPath = "/uploads/" + imgFile[i].FileName;
                    }

                    var newFood = new FoodItem();
                    newFood.FoodName = FoodName[i];
                    newFood.Quantity = Quantity[i];
                    newFood.Price = Price[i];
                    newFood.ExpireDate = ExpireDate[i];
                    newFood.ImagePath = imgPath;
                    newFood.Quantity = Quantity[i];
                    newFood.MenuId = model.MenuId;
                    ctx.foodItems.Add(newFood);
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var ctx = new Model1();
            var oFood = ctx.foodItems.Where(p => p.ItemId == id).FirstOrDefault();
            if (oFood != null)
            {
                ctx.foodItems.Remove(oFood);
                ctx.SaveChanges();

                var fileName = Path.GetFileName(oFood.ImagePath);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);
                // Check if file exists with its full path    
                if (System.IO.File.Exists(fileLocation))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(fileLocation);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteMultiple(int id)
        {
            var ctx = new Model1();
            var listFood = ctx.foodItems.Where(p => p.MenuId == id).ToList();
            foreach (var oFood in listFood)
            {
                if (oFood != null)
                {
                    ctx.foodItems.Remove(oFood);
                    ctx.SaveChanges();

                    var fileName = Path.GetFileName(oFood.ImagePath);
                    string fileLocation = Path.Combine(
                        Server.MapPath("~/uploads"), fileName);
                    // Check if file exists with its full path    
                    if (System.IO.File.Exists(fileLocation))
                    {
                        // If file found, delete it    
                        System.IO.File.Delete(fileLocation);
                    }
                }
            }

            var oMenu = ctx.Menus.Where(c => c.MenuId == id).FirstOrDefault();
            ctx.Menus.Remove(oMenu);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}