//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebDauGia.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int ProID { get; set; }
        public string ProName { get; set; }
        public int CatID { get; set; }
        public string FullDes { get; set; }
        public string TinyDes { get; set; }
        public string Salesman { get; set; }
        public Nullable<double> Price { get; set; }
        public int Quantity { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int StepPrice { get; set; }
        public double AucPrice { get; set; }
        public string Owner { get; set; }
        public Nullable<double> OwnerPrice { get; set; }
        public bool Status { get; set; }
        public int NumOfAuction { get; set; }
        public bool AutoRenewal { get; set; }
    }
}
