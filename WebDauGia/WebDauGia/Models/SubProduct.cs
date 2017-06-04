using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauGia.Models
{
    public class SubProduct
    {
        public int ProID { get; set; }
        public string ProName { get; set; }
        public string CatId { get; set; }
        // public string Quantity { get; set; }
        public double Price { get; set; }
        public double PurchasePriceNow { get; set; }
        public string TinyDes { get; set; }
        public string FullDes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Buyer { get; set; }
        public int PointBuyer { get; set; }
        public string Seller { get; set; }
        public int PointSeller { get; set; }
        public int NumOfAuction { get; set; }
        //public string StepPrice { get; set; }
    }
}