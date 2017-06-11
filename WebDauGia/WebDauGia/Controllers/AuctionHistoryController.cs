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
        //[ValidateInput(false)]
        public ActionResult Add(double giatra, int proid)
        {
            int TH = 0;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                var pro = ctx.Products.Where(p => p.ProID == proid).FirstOrDefault();
                var auhis = new AuctionHistory();
                auhis.ProID = proid;
                auhis.UserName = ((User)Session["user"]).UserName;
                auhis.AucPrice = giatra;
                auhis.Time = DateTime.Now;
                ctx.AuctionHistorys.Add(auhis);
                ctx.SaveChanges();
                //tang lượt đâu giá lên 1
                pro.NumOfAuction += 1;
                if(giatra >=pro.Price)
                {
                    TH = 3;//chiến thắng tuyệt đối
                    //update Product
                    pro.OwnerPrice = pro.Price;
                    pro.AucPrice = pro.Price;
                    pro.EndTime = DateTime.Now;
                    ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                else if(giatra > pro.OwnerPrice)
                {
                    TH=1;// 1 là chiến thắng.
                    //update Product
                    pro.OwnerPrice = giatra;
                    pro.AucPrice += pro.StepPrice;
                    pro.Owner = ((User)Session["user"]).UserName;
                    ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                else if(giatra <= pro.OwnerPrice)
                {
                    TH = 2;//2 la thua 
                    //update Product
                    pro.AucPrice = giatra;
                    ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
            }
            //return RedirectToAction("Detail", "Product", new { ID = a.ProID });
            return Json(TH, JsonRequestBehavior.AllowGet);
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