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
    
    public partial class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Credit { get; set; }
        public System.DateTime DateCreate { get; set; }
        public bool AllowAuction { get; set; }
        public bool AllowSales { get; set; }
        public string Reliability { get; set; }
        public int UserID { get; set; }
    }
}
