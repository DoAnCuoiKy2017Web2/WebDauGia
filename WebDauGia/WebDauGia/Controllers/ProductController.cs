using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Filters;
using WebDauGia.Helper;
using WebDauGia.Models;

namespace WebDauGia.Controllers
{
    
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        //Top 5 Theo Gia
        public ActionResult Top5Price()
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<Product> list = ctx.Products.OrderByDescending(l => l.AucPrice).Where(l => l.EndTime > DateTime.Now)
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
        [HttpGet, ValidateInput(false)]
        public ActionResult ByCat(int? id, int Command = 0, int page = 1)
        {
            @ViewBag.Command = Command;
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }
            if(id == -1)
            {
                using (var ctx = new QuanLyDauGiaEntities())
                {
                    @ViewBag.curPage = page;
                    @ViewBag.CatName = "Tất cả";

                    int n = ctx.Products.Where(p => p.EndTime > DateTime.Now).Count();

                    int recordsPerPage = 3;
                    int nPages = n / recordsPerPage + (n % recordsPerPage == 0 ? 0 : 1);

                    @ViewBag.Pages = nPages;
                    if (Command == 1)
                    {
                        List<Product> list = ctx.Products
                       .Where(p => p.EndTime > DateTime.Now)
                       .OrderByDescending(p => p.EndTime)
                       .Skip((page - 1) * recordsPerPage)
                       .Take(recordsPerPage)
                       .ToList();
                        return View(list);
                    }
                    else if (Command == 2)
                    {
                        List<Product> list = ctx.Products
                        .Where(p => p.EndTime > DateTime.Now)
                        .OrderBy(p => p.AucPrice)
                        .Skip((page - 1) * recordsPerPage)
                        .Take(recordsPerPage)
                        .ToList();
                        return View(list);
                    }
                    else
                    {
                        List<Product> list = ctx.Products
                        .Where(p => p.EndTime > DateTime.Now)
                        .OrderBy(p => p.ProID)
                        .Skip((page - 1) * recordsPerPage)
                        .Take(recordsPerPage)
                        .ToList();
                        return View(list);
                    }
                }
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

                int n = ctx.Products.Where(p => p.CatID == id && p.EndTime > DateTime.Now).Count();

                int recordsPerPage = 3;
                int nPages = n / recordsPerPage + (n % recordsPerPage == 0 ? 0 : 1);

                @ViewBag.Pages = nPages;

                if (Command == 1)
                {
                    List<Product> list = ctx.Products
                    .Where(p => p.CatID == id && p.EndTime > DateTime.Now)
                    .OrderByDescending(p => p.EndTime)
                    .Skip((page - 1) * recordsPerPage)
                    .Take(recordsPerPage)
                    .ToList();
                    return View(list);
                }
                else if (Command == 2)
                {
                    List<Product> list = ctx.Products
                   .Where(p => p.CatID == id && p.EndTime > DateTime.Now)
                   .OrderBy(p => p.AucPrice)
                   .Skip((page - 1) * recordsPerPage)
                   .Take(recordsPerPage)
                   .ToList();
                    return View(list);
                }
                else
                {
                    List<Product> list = ctx.Products
                    .Where(p =>p.CatID == id && p.EndTime > DateTime.Now)
                    .OrderBy(p => p.ProID)
                    .Skip((page - 1) * recordsPerPage)
                    .Take(recordsPerPage)
                    .ToList();
                    return View(list);
                }
            }
        }
        //GET:Product/Search
        //[HttpGet, ValidateInput(false)]
        public ActionResult Search(string txtkey, int Command = 0, int page = 1)
        {
            @ViewBag.key = txtkey;
            @ViewBag.Command = Command;
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
                            select p).Where(p=>p.EndTime>DateTime.Now).ToList();
                var kq2 = ctx.Products.Where(p => p.ProName.ToLower().Contains(txtkey.ToLower()) && p.EndTime>DateTime.Now).ToList();
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
                if (Command == 1)
                {
                   LSP=LSP.OrderByDescending(l => l.EndTime).ToList();
                }
                else if (Command == 2)
                {
                    LSP = LSP.OrderBy(l => l.AucPrice).ToList();
                }
                int n =LSP.Count();
                int recordsPerPage = 3;
                int nPages = n / recordsPerPage + (n % recordsPerPage == 0 ? 0 : 1);

                @ViewBag.Pages = nPages;
                ViewBag.key = txtkey;
                List<Product> list = LSP.Skip((page - 1) * recordsPerPage)
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
        [CheckLogin]
        public ActionResult Add()
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<Category> list = ctx.Categories.ToList();
                @ViewBag.DanhSachDanhMuc = list;
            }
            ProductVM model = new ProductVM()
            {
                ProName = "",
                CatId = "",
                Quantity="",
                Price="",
                AucPrice="",
                TinyDes="",
                FullDes="",
                StartTime = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year,
                EndTime = String.Format("{0:d/M/yyyy hh:mm tt}", DateTime.Now.AddDays(7)),
                StepPrice = "",
                AutoRenewal = "True",
            };
            return View(model);
        }

        //Post : Produc/Add
        [CheckLogin]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(ProductVM pro, HttpPostedFileBase fuMain, HttpPostedFileBase fuThumbs_1, HttpPostedFileBase fuThumbs_2)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<Category> list = ctx.Categories.ToList();
                @ViewBag.DanhSachDanhMuc = list;
            }
            if (double.Parse(pro.AucPrice) % 1000 != 0 || double.Parse(pro.AucPrice) == 0)
            {
                if (double.Parse(pro.AucPrice) < 1000)
                {                  
                    Response.Write("<script LANGUAGE='JavaScript' >alert('Giá khởi điểm phải lớn hơn 1000!!')</script>");
                }
                else
                {
                    Response.Write("<script LANGUAGE='JavaScript' >alert('Giá khởi điểm phải chia hết cho 1000!!')</script>");
                }               
                View(pro);
            }
            else
            {
                Product model = new Product();
                model.Salesman = CurrentContext.GetCurUser().UserName;
                using (var ctx = new QuanLyDauGiaEntities())
                {

                    model.ProName = pro.ProName;
                    model.CatID = int.Parse(pro.CatId);
                    model.Quantity = int.Parse(pro.Quantity);
                    model.AucPrice = double.Parse(pro.AucPrice);
                    if (pro.Price != null && pro.Price != "")
                    {
                        model.Price = double.Parse(pro.Price);
                        if (model.Price <= model.AucPrice)
                        {
                            Response.Write("<script LANGUAGE='JavaScript' >alert('Giá mua ngay phải lớn hơn giá khởi điểm!!')</script>");
                            View(pro);
                            return View(pro);
                        }
                        if (model.Price % 1000 != 0)
                        {
                            Response.Write("<script LANGUAGE='JavaScript' >alert('Giá mua ngay phải chia hết cho 1000!!')</script>");
                            View(pro);
                            return View(pro);
                        }
                    }
                    if (pro.StepPrice == null || pro.StepPrice == "")
                    {
                        if (model.AucPrice < 10000)
                        {
                            model.StepPrice = 1000;
                        }
                        else
                        {
                            model.StepPrice = ((int)model.AucPrice / 100 * 10) / 1000 * 1000; // 10% giá khởi điểm.
                        }
                    }
                    else
                    {
                        model.StepPrice = (int)(double.Parse(pro.StepPrice));
                    }
                    model.AutoRenewal = pro.AutoRenewal == "True" ? true : false;
                    model.TinyDes = pro.TinyDes;
                    model.FullDes = pro.FullDes;
                    model.StartTime = DateTime.ParseExact(pro.StartTime, "dd/MM/yyyy", null);
                    model.EndTime = DateTime.ParseExact(pro.EndTime, "dd/MM/yyyy hh:mm tt", null);
                    model.Status = false;
                    if (model.StartTime <= DateTime.Now)
                        model.StartTime = DateTime.Now;
                    ctx.Entry(model).State = System.Data.Entity.EntityState.Added;
                    //ctx.SaveChanges();
                    @ViewBag.Message = "Đã thêm thành công.";
                    List<Category> list = ctx.Categories.ToList();
                    @ViewBag.DanhSachDanhMuc = list;
                }
                //tạo foder chứa hình
                string spDirPath = Server.MapPath("~/Images/sp");
                string targetDirPath = Path.Combine(spDirPath, model.ProID.ToString());
                Directory.CreateDirectory(targetDirPath);
                if (fuMain != null && fuMain.ContentLength > 0)
                {
                    //copy hình
                    string mainFileName = Path.Combine(targetDirPath, "main.jpg");
                    WebDauGia.Helper.Picture.SaveResizeImage(Image.FromStream(fuMain.InputStream), 400, 300, mainFileName);
                    //fuMain.SaveAs(mainFileName);

                }
                if (fuThumbs_1 != null && fuThumbs_1.ContentLength > 0)
                {
                    string thumbs_1FileName = Path.Combine(targetDirPath, "main_thumbs_1.jpg");
                    WebDauGia.Helper.Picture.SaveResizeImage(Image.FromStream(fuMain.InputStream), 200, 150, thumbs_1FileName);
                    //fuThumbs_1.SaveAs(thumbs_1FileName);
                }
                if (fuThumbs_2 != null && fuThumbs_2.ContentLength > 0)
                {
                    string thumbs_2FileName = Path.Combine(targetDirPath, "main_thumbs_2.jpg");
                    WebDauGia.Helper.Picture.SaveResizeImage(Image.FromStream(fuMain.InputStream), 200, 150, thumbs_2FileName);
                    //fuThumbs_2.SaveAs(thumbs_2FileName);
                }
            }
            return View(pro);
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
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateFullDes(string ProID, String FullDes)
        {
            int ID = int.Parse(ProID);
            using (var ctx = new QuanLyDauGiaEntities())
            {
                Product sp = ctx.Products
                    .Where(p => p.ProID == ID)
                    .FirstOrDefault();
                sp.FullDes = sp.FullDes + "\n\n EDIT (" + String.Format("{0:d/M/yyyy}", DateTime.Now) + ")\n" + FullDes;
                ctx.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
            Response.Write("<script LANGUAGE='JavaScript' >alert('Đã cập nhật.')</script>");
            return RedirectToAction("UnexpiredProducts", "User");
        }
        
    }
}