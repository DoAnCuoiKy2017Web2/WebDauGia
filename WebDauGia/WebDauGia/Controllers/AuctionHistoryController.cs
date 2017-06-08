using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Models;

namespace WebDauGia.Controllers
{
    public class AuctionHistoryController : Controller
    {
        // GET: AuctionHistory
        public ActionResult Index()
        {
            return View();
        }
        //Get : AuctionHistory/Add
        public ActionResult Add()
        {
            return View();
        }

        //Post : AuctionHistory/Add
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(int proid ,string uname,DateTime time, double ragia)
        {
            AuctionHistory a = new AuctionHistory();
            a.ProID = proid;
            a.UserName = uname;
            a.Time = time;
            a.AucPrice = ragia;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(a).State = System.Data.Entity.EntityState.Added;
                ctx.SaveChanges();
                @ViewBag.Message = "Đã thêm thành công.";
            }
            return RedirectToAction("Detail", "Product", new { ID = a.ProID });
        }

        ////Post : AuctionHistory/Add
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Add(AuctionHistory model)
        //{
        //    using (var ctx = new QuanLyDauGiaEntities())
        //    {
        //        ctx.Entry(model).State = System.Data.Entity.EntityState.Added;
        //        ctx.SaveChanges();
        //        @ViewBag.Message = "Đã thêm thành công.";
        //    }
        //    return View();
        //}

        //Get : AuctionHistory/Edit
        public ActionResult Edit(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "AuctionHistory");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                AuctionHistory auch = dt.AuctionHistorys
                    .Where(p => p.AucID == ID)
                    .FirstOrDefault();
                if (auch != null)
                {
                    return View(auch);
                }
                return RedirectToAction("Index", "AuctionHistory");
            }
        }

        //Post: AuctionHistory/Update
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(AuctionHistory model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                @ViewBag.Message = "Cập nhật thành công.";
            }
            return RedirectToAction("Edit", "AuctionHistory", new { ID = model.AucID });
        }

        //Get : AuctionHistory/Delete
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "AuctionHistory");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                AuctionHistory auch = dt.AuctionHistorys
                    .Where(p => p.ProID == ID)
                    .FirstOrDefault();
                return View(auch);
            }
        }

        //Post : AuctionHistory/Deleted
        [HttpPost]
        public ActionResult Deleted(int ID)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                AuctionHistory auch = ctx.AuctionHistorys
                    .Where(p => p.AucID == ID)
                    .FirstOrDefault();
                ctx.Entry(auch).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "AuctionHistory");
        }
    }
}