﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDauGia.Models;


namespace WebDauGia.Helper
{
    public class CurrentContext
    {
        public static bool IsLogged()
        {
            var flag = HttpContext.Current.Session["isLogin"];
            if (flag == null || (int)flag == 0)
            {
                if (HttpContext.Current.Request.Cookies["userID"] != null)
                {
                    string userIdCookie = Convert.ToString(HttpContext.Current.Request.Cookies["userID"].Value);
                    using (var ctx = new QuanLyDauGiaEntities())
                    {
                        var user = ctx.Users.Where(u => u.UserName == userIdCookie).FirstOrDefault();


                        HttpContext.Current.Session["isLogin"] = 1;
                        HttpContext.Current.Session["user"] = user;
                        
                    }
                    return true;
                }
                return false;
            }
            return true;    
        }

        public static User GetCurUser()
        {
            return (Models.User)HttpContext.Current.Session["user"];
        }

        public static void Destroy()
        {
            HttpContext.Current.Session["isLogin"] = 0;
            HttpContext.Current.Session["user"] = null;
            //HttpContext.Current.Session["cart"] = null;

            HttpContext.Current.Response.Cookies["userID"].Expires = DateTime.Now.AddDays(-1);
        }

        public static bool IsAdminLogged()
        {
            var flag = HttpContext.Current.Session["isAdminLogin"];
            if (flag == null || (int)flag == 0)
            {
                if (HttpContext.Current.Request.Cookies["adminID"] != null)
                {
                    string userIdCookie = Convert.ToString(HttpContext.Current.Request.Cookies["adminID"].Value);
                    using (var ctx = new QuanLyDauGiaEntities())
                    {
                        var user = ctx.Users.Where(u => u.UserName == userIdCookie).FirstOrDefault();


                        HttpContext.Current.Session["isAdminLogin"] = 1;
                        HttpContext.Current.Session["admin"] = user;

                    }
                    return true;
                }
                return false;
            }
            return true;
        }
        public static void AdDestroy()
        {
            HttpContext.Current.Session["isAdminLogin"] = 0;
            HttpContext.Current.Session["admin"] = null;
        }
        public static Admin GetCurAdmin()
        {
            return (Admin)HttpContext.Current.Session["admin"];
        }
    }
}