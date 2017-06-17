using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Helper;

namespace WebDauGia.Filters
{
    public class CheckAdminLoginAttribute : ActionFilterAttribute
    {
        public int RequiredPermission { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentContext.IsAdminLogged() == false)
            {
                //string controller = filterContext.RouteData.Values["controller"].ToString();
                //string action = filterContext.RouteData.Values["action"].ToString();

                //filterContext.Result = new RedirectResult(string.Format("~/Account/Login?retUrl=/{0}/{1}", controller, action));
                filterContext.Result = new RedirectResult("~/Admin/Login");
                return;
            }

            //if (CurrentContext.GetCurUser().f_Permission < this.RequiredPermission)
            //{
            //    filterContext.Result = new HttpUnauthorizedResult();
            //    return;
            //}

            base.OnActionExecuting(filterContext);
        }
    }
}