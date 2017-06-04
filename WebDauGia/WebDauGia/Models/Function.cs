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
    }
}