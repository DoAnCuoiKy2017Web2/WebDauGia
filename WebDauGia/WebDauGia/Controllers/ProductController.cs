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
    }
}