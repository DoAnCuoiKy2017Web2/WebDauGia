using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebDauGia.Helper
{
    public class StringUtils
    {
        public static string MD5(string strinput)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] input = Encoding.Default.GetBytes(strinput);
            byte[] output = md5.ComputeHash(input);
            string ret = BitConverter.ToString(output).Replace("-", "");
            return ret;
        }
    }
}