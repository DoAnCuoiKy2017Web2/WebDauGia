using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Models;

namespace WebDauGia.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<Category> list = ctx.Categories.ToList();
                return PartialView("ListPartial", list);
            }
        }
        //GET: Category/Add
        public ActionResult Add()
        {
            return View();
        }

        //Post : Category/Add
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Category model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Added;
                ctx.SaveChanges();
                @ViewBag.Message = "Đã thêm thành công.";
            }
            return View();
        }
        //Get : Category/Edit
        public ActionResult Edit(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Category");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                Product sp = dt.Products
                    .Where(p => p.ProID == ID)
                    .FirstOrDefault();
                if (sp != null)
                {
                    Category danhmuc = dt.Categories
                    .Where(p => p.CatID == sp.CatID)
                    .FirstOrDefault();
                    return View(sp);
                }
                return RedirectToAction("Index", "Category");
            }
        }

        //Post: Category/Update
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Category model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                @ViewBag.Message = "Cập nhật thành công.";
            }
            return RedirectToAction("Edit", "Category", new { ID = model.CatID });
        }

        //Get : Category/Delete
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Category");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                Category sp = dt.Categories
                    .Where(p => p.CatID == ID)
                    .FirstOrDefault();
                return View(sp);
            }
        }

        //Post : Category/Deleted
        [HttpPost]
        public ActionResult Deleted(int ID)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                Category sp = ctx.Categories
                    .Where(p => p.CatID == ID)
                    .FirstOrDefault();
                ctx.Entry(sp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "Produc");
        }
    }
}