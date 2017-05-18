using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauGia.Models
{
    public class LoginVM
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Remember { get; set; }
    }
}