using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauGia.Models
{
    public class WatchListVM
    {
        public string ProName { get; set; }
        public int ProID { get; set; }
        public double Price { get; set; }
        public double AucPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public String UserName { get; set; }
    }
}