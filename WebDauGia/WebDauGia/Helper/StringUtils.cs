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

        //Mã hóa tên người dùng.
        public static string Replace(string strinput)
        {
            if (strinput == null)
                return "";
            if (strinput.Length <= 3)
                return strinput.Replace(strinput.Substring(0, 1), "*****");
            else
                return strinput.Replace(strinput.Substring(0, strinput.Length - 3), "*****");
        }
    }
}