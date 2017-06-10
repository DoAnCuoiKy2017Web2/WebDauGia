using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauGia.Models
{
    public class ChangePasswordVM
    {
        public string UserName { get; set; }
        public string OldPassWord { get; set; }
        public string NewPassWord { get; set; }
    }
}