using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Models;

namespace WebDauGia.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        //GET: Product/ByCat
        public ActionResult ByCat(int? id, int page = 1)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }
            using (var ctx = new QuanLyDauGiaEntities())
            {
                @ViewBag.curPage = page;
                string c = ctx.Categories
                    .Where(p => p.CatID == id)
                    .FirstOrDefault()
                    .CatName
                    .ToString();
                @ViewBag.CatName = c;

                int n = ctx.Products.Where(p => p.CatID == id).Count();

                int recordsPerPage = 3;
                int nPages = n / recordsPerPage + (n % recordsPerPage == 0 ? 0 : 1);

                @ViewBag.Pages = nPages;

                List<Product> list = ctx.Products
                    .Where(p => p.CatID == id)
                    .OrderBy(p => p.ProID)
                    .Skip((page - 1) * recordsPerPage)
                    .Take(recordsPerPage)
                    .ToList();

                return View(list);
            }
        }

        //Get : Produc/Add
        public ActionResult Add()
        {
            return View();
        }

        //Post : Produc/Add
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Product model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Added;
                ctx.SaveChanges();
                @ViewBag.Message = "Đã thêm thành công.";
            }
            return View();
        }

        //Get : Produc/Edit
        public ActionResult Edit(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Produc");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                Product sp = dt.Products
                    .Where(p => p.ProID == ID)
                    .FirstOrDefault();
                if (sp != null)
                {
                    return View(sp);
                }
                return RedirectToAction("Index", "Produc");
            }
        }

        //Post: Product/Update
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Product model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                @ViewBag.Message = "Cập nhật thành công.";
            }
            return RedirectToAction("Edit", "Produc", new { ID = model.ProID });
        }

        //Get : Produc/Delete
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Produc");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                Product sp = dt.Products
                    .Where(p => p.ProID == ID)
                    .FirstOrDefault();
                return View(sp);
            }
        }

        //Post : Produc/Deleted
        [HttpPost]
        public ActionResult Deleted(int ID)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                Product sp = ctx.Products
                    .Where(p => p.ProID == ID)
                    .FirstOrDefault();
                ctx.Entry(sp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "Produc");
        }

    }
}