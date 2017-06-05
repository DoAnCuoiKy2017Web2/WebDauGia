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
        //Top 5 Theo Gia
        public ActionResult Top5Price()
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<Product> list = ctx.Products.OrderByDescending(l => l.Price).Where(l => l.EndTime > DateTime.Now)
                //    .Select(l => new SubProduct
                //{
                //    ProID = l.ProID,
                //    ProName = l.ProName,
                //    Price = l.Price,
                //    Buyer = l.Owner.Replace(l.Owner.Substring(0, 3), "***"),
                //    StartTime = l.StartTime,
                //    EndTime = l.EndTime,
                //    NumOfAuction = l.NumOfAuction
                //})
                .Take(5).ToList();
                return PartialView("Top5Price", list);
            }
        }
        //top 5 theo luot mua
        public ActionResult Top5NumAuction()
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<Product> list = ctx.Products.OrderByDescending(l => l.NumOfAuction).Where(l => l.EndTime > DateTime.Now)
                //    .Select(l => new SubProduct
                //{
                //    ProID = l.ProID,
                //    ProName = l.ProName,
                //    Price = l.Price,
                //    Buyer = l.Owner.Replace(l.Owner.Substring(0, 3), "***"),
                //    StartTime = l.StartTime,
                //    EndTime = l.EndTime,
                //    NumOfAuction = l.NumOfAuction
                //})
                .Take(5).ToList();
                return PartialView("Top5NumAuction", list);
            }
        }
        //top 5 gan ket thuc
        public ActionResult Top5MinTime()
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<Product> list = ctx.Products.OrderBy(l => l.EndTime).Where(l => l.EndTime > DateTime.Now)
                //    .Select(l => new SubProduct
                //{
                //    ProID = l.ProID,
                //    ProName = l.ProName,
                //    Price = l.Price,
                //    Buyer = l.Owner.Replace(l.Owner.Substring(0, 3), "***"),
                //    StartTime = l.StartTime,
                //    EndTime = l.EndTime,
                //    NumOfAuction = l.NumOfAuction
                //})
                .Take(5).ToList();
                return PartialView("Top5MinTime", list);
            }
        }
        //GET: Product/ByCat//chinh sua ngày 4/6/2017
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
                    //.Select(l => new SubProduct
                    //{
                    //    ProID = l.ProID,
                    //    ProName = l.ProName,
                    //    Price = l.Price,
                    //    Buyer = l.Owner.Replace(l.Owner.Substring(0, 3), "***"),
                    //    StartTime = l.StartTime,
                    //    EndTime = l.EndTime,
                    //    NumOfAuction = l.NumOfAuction
                    //})
                    .Skip((page - 1) * recordsPerPage)
                    .Take(recordsPerPage)
                    .ToList();
                return View(list);
            }
        }
        //GET:Product/Search
        public ActionResult Search(string txtkey, int page = 1)
        {
            @ViewBag.key = txtkey;
            if (txtkey == "")
            {
                return RedirectToAction("Index", "Home");
            }
            using (var ctx = new QuanLyDauGiaEntities())
            {
                @ViewBag.curPage = page;

                List<Product> LSP = new List<Product>();
                var Cat = ctx.Categories.Where(c=>c.CatName.ToLower().Contains(txtkey.ToLower())).ToList();
                var kq1 = (from c in Cat
	                      join p in ctx.Products on c.CatID equals p.CatID
                            select p).ToList();
                var kq2 = ctx.Products.Where(p => p.ProName.ToLower().Contains(txtkey.ToLower())).ToList();
                LSP.AddRange(kq1);
                LSP.AddRange(kq2);
                for(int i=0;i<LSP.Count()-1;i++)
                {
                    for(int j=i+1;j<LSP.Count();j++)
                    {
                        if(LSP[i].ProID==LSP[j].ProID)
                        {
                            LSP.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
                int n =LSP.Count();
                int recordsPerPage = 3;
                int nPages = n / recordsPerPage + (n % recordsPerPage == 0 ? 0 : 1);

                @ViewBag.Pages = nPages;
                ViewBag.key = txtkey;
                List<Product> list = LSP.OrderBy(l=>l.ProID).Skip((page - 1) * recordsPerPage)
                    .Take(recordsPerPage).ToList();
                return View(list);
            }
        }
        //GET: Product/Detail
        public ActionResult Detail(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            using (var ctx = new QuanLyDauGiaEntities())
            {
                var model = ctx.Products.Where(p => p.ProID == id).FirstOrDefault();
                return View(model);
            }
        }
        

        //Get : Produc/Add
        public ActionResult Add()
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<Category> list = ctx.Categories.ToList();
                @ViewBag.DanhSachDanhMuc = list;
            }
            return View();
        }

        //Post : Produc/Add
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(ProductVM pro)
        {
            Product model = new Product();
            if (model.Salesman == null)
                model.Salesman = "Admin"; // Sau này đăng nhập sẽ làm lại dòng này.
            
            if (model.StepPrice == 0)
            {
                int price =(int)(double.Parse(pro.Price));
                model.StepPrice = price / 100 * 10; // 10% giá gốc
            }
            using (var ctx = new QuanLyDauGiaEntities())
            {
                //model.AucPrice = 0;
                //model.OwnerPrice = 100;
                //model.Owner = "admin";

                model.ProName = pro.ProName;
                model.CatID = int.Parse(pro.CatId);
                model.Quantity = int.Parse(pro.Quantity);
                model.Price = double.Parse(pro.Price);
                model.TinyDes = pro.TinyDes;
                model.FullDes = pro.FullDes;
                model.StartTime = DateTime.ParseExact(pro.StartTime, "d/M/yyyy", null);
                model.EndTime = DateTime.ParseExact(pro.EndTime, "d/M/yyyy", null);

                ctx.Entry(model).State = System.Data.Entity.EntityState.Added;
                ctx.SaveChanges();
                @ViewBag.Message = "Đã thêm thành công.";
                List<Category> list = ctx.Categories.ToList();
                @ViewBag.DanhSachDanhMuc = list;
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