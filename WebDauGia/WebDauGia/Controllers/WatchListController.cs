using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Filters;
using WebDauGia.Helper;
using WebDauGia.Models;
namespace WebDauGia.Controllers
{
    public class WatchListController : Controller
    {
        // GET: WatchList
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(int ProID)
        {
            if(CurrentContext.IsLogged()==false)
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else
            {
                string username = ((User)Session["user"]).UserName;
                using(var ctx=new QuanLyDauGiaEntities())
                {
                    var wl = ctx.WatchLists.Where(w => w.ProID == ProID && w.UserName == username).FirstOrDefault();
                    if (wl == null)
                    {
                        var wlnew = new WatchList();
                        wlnew.UserName = username;
                        wlnew.ProID = ProID;
                        ctx.WatchLists.Add(wlnew);
                        ctx.SaveChanges();
                        return Json("Đã Thêm Vào Danh Sách Yêu Thích!!!", JsonRequestBehavior.AllowGet);
                    }
                    else
                return Json("Sản Phẩm Này Đã Tồn Tại Trong List Của Bạn Rồi!!!", JsonRequestBehavior.AllowGet);

                }
            }
        }
        public ActionResult List()
        {
            string username = ((User)Session["user"]).UserName;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<WatchListVM> list = (from p in ctx.Products
                                          join w in ctx.WatchLists on p.ProID equals w.ProID
                                          where w.UserName == username
                                          select new WatchListVM
                                          {
                                              ProID = p.ProID,
                                              ProName = p.ProName,
                                              UserName = username,
                                              Price =p.Price,
                                              AucPrice=(double)p.AucPrice,
                                              EndTime =p.EndTime,
                                              StartTime=p.StartTime
                                          }).ToList();
                return PartialView("ListPartial", list);
            }
        }
        public ActionResult ListDangThamGia()
        {
            string username = ((User)Session["user"]).UserName;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<WatchListVM> list = (from p in ctx.Products
                                          join h in ctx.AuctionHistorys on p.ProID equals h.ProID
                                          where h.UserName == username && p.EndTime < DateTime.Now
                                          select new WatchListVM
                                          {
                                              ProID = p.ProID,
                                              ProName = p.ProName,
                                              UserName = username,
                                          }).ToList();
                return PartialView("ListPartial", list);
            }
        }
        public ActionResult ListDaThang()
        {
            string username = ((User)Session["user"]).UserName;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                List<WatchListVM> list = (from p in ctx.Products
                                          where p.Owner == username && p.EndTime >= DateTime.Now
                                          select new WatchListVM
                                          {
                                              ProID = p.ProID,
                                              ProName = p.ProName,
                                              UserName = username,
                                          }).ToList();
                return PartialView("ListPartial", list);
            }
        }
    }
}