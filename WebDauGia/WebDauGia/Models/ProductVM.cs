using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauGia.Models
{
    public class ProductVM
    {
        public string ProName { get; set; }
        public string CatId { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string AucPrice { get; set; }

        public string TinyDes { get; set; }
        public string FullDes { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StepPrice { get; set; }
    }
}