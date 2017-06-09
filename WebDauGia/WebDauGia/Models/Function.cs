using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauGia.Models
{
    public class Function
    {
        public static String DateTimeToString(DateTime Finish)
        {
            DateTime Today = DateTime.Now;
            TimeSpan kq = Finish - Today;
            return kq.Days + "d:" + kq.Hours + "h:" + kq.Minutes + "p.";
        }
        public static int CheckNew(DateTime Start)
        {
            int check = 0;
            DateTime Today = DateTime.Now;
            TimeSpan kq = Today - Start;
            if (kq.Days * 24 * 60 + kq.Hours * 60 + kq.Minutes <= 6000)
            {
                check = 1;
            }
            return check;
        }
        public static double GetReliability(String username)
        {
            double diemdanhgia = 0;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                User us = ctx.Users
                    .Where(p => p.UserName == username)
                    .FirstOrDefault();
                if(us != null)
                {
                    String []diemdanhgiaArr = us.Reliability.Split('/');
                    diemdanhgia = double.Parse(diemdanhgiaArr[0]) / (double.Parse(diemdanhgiaArr[0]) + double.Parse(diemdanhgiaArr[1]));
                }
            }
            return diemdanhgia * 100;
        }
    }
    

}