using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Filters;
using WebDauGia.Helper;
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
                return Json("Vui Lòng Đăng Nhập Để thực hiện chức năng này", JsonRequestBehavior.AllowGet);
            }
            return Json("Muốn Gì Mày",JsonRequestBehavior.AllowGet);
        }
    }
}