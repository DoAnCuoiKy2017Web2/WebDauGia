using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static int CheckNew(DateTime Start)
        {
            int check = 0;
            DateTime Today = DateTime.Now;
            TimeSpan kq = Today - Start;
            if (kq.Days * 24 * 60 + kq.Hours * 60 + kq.Minutes <= 2400)
            {
                check = 1;
            }
            return check;
        }
        public static int CheckTime(DateTime d)
        {
            int check = 0;
            DateTime Today = DateTime.Now;
            TimeSpan kq = Today - d;
            if (kq.Days * 24 * 60 + kq.Hours * 60 + kq.Minutes <= 0)
            {
                check = 1;
            }
            return check;
        }
        public static int Rest(DateTime d)
        {

            DateTime Today = DateTime.Now;
            TimeSpan kq =d-Today;
            return (kq.Days * 24 + kq.Hours)*3600 + kq.Minutes * 60 + kq.Seconds;
        }
        public static double GetReliability(String username)
        {
            double diemdanhgia = 0;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                User us = ctx.Users
                    .Where(p => p.UserName == username)
                    .FirstOrDefault();
                if(us != null)
                {
                    String []diemdanhgiaArr = us.Reliability.Split('/');
                    diemdanhgia = double.Parse(diemdanhgiaArr[0]) / (double.Parse(diemdanhgiaArr[0]) + double.Parse(diemdanhgiaArr[1]));
                }
            }
            return diemdanhgia * 100;
        }
        public static StringBuilder GmailDuocQuyenGiuGia(Product pro, string username)
        {
            StringBuilder Body = new StringBuilder();
            Body.Append("<h3>Chào: <b>" + username + "<b></h3>");
            Body.Append("<p>Vào Lúc " + DateTime.Now.ToString() + " Bạn Đã Trở Thành Người Giữ Giá Sản Phẩm: " + pro.ProName + "</p>");
            Body.Append("<p>Giá Bạn Trả Cho Sản Phẩm Này Là: " + string.Format("{0:N0}", pro.OwnerPrice) + " VNĐ</p>");
            Body.Append("<p style='color:red'>Lưu ý: giá bạn phải trả cho sản phẩm này sẻ là giá hiện tại của sản phẩm khi phiên đấu giá kết thúc!</p>");
            Body.Append("<table>");
            Body.Append("<tr><td colspan='2'><h4>Thông tin Sản Phẩm</h4></td></tr>");
            Body.Append("<tr><td>Tên Sản Phẩm: </td><td>" + pro.ProName + "</td></tr>");
            Body.Append("<tr><td>Giá Hiện Tại: </td><td>" + string.Format("{0:N0}", pro.AucPrice) + " VNĐ</td></tr>");
            Body.Append("<tr><td>Thời Gian Kết Thúc Đấu Giá: </td><td>" + pro.EndTime.ToString() + "</td></tr>");
            Body.Append("</table>");
            return Body;
            //MailMessage mail = new MailMessage();
            //mail.To.Add("Dragonblue789@gmail.com");
            //mail.From = new MailAddress("admiweb2nhom5@gmail.com");
            //mail.Subject = "Thông Báo Giữ Giá Sản Phẩm "+ pro.ProName;
            //mail.Body = Body.ToString();// phần thân của mail ở trên
            //mail.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
            //smtp.EnableSsl = true;
            //smtp.Send(mail);
        }
        public static StringBuilder GmailMatQuyenGiuGia(Product pro, string username)
        {
            StringBuilder Body = new StringBuilder();
            Body.Append("<h3>Chào: <b>" + username + "<b></h3>");
            Body.Append("<p>Vào Lúc " + DateTime.Now.ToString() + " Có Người Trả Giá Cao Hơn Bạn (Bạn Đã Mất Quyền Giữ Giá)</p>");
            Body.Append("<table>");
            Body.Append("<tr><td colspan='2'><h4>Thông tin Sản Phẩm</h4></td></tr>");
            Body.Append("<tr><td>Tên Sản Phẩm: </td><td>" + pro.ProName + "</td></tr>");
            Body.Append("<tr><td>Giá Hiện Tại: </td><td>" + string.Format("{0:N0}", pro.AucPrice) + " VNĐ</td></tr>");
            Body.Append("<tr><td>Thời Gian Kết Thúc Đấu Giá: </td><td>" + pro.EndTime.ToString() + "</td></tr>");
            Body.Append("</table>");
            return Body;
            //MailMessage mail = new MailMessage();
            //mail.To.Add("Dragonblue789@gmail.com");
            //mail.From = new MailAddress("admiweb2nhom5@gmail.com");
            //mail.Subject = "Thông Báo Bị Cướp Quyền Giá Sản Phẩm " + pro.ProName;
            //mail.Body = Body.ToString();// phần thân của mail ở trên
            //mail.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");// tài khoản Gmail của bạn
            //smtp.EnableSsl = true;
            //smtp.Send(mail);
        }
        public static StringBuilder GmailTBChienThang(Product pro, string username)
        {
            StringBuilder Body = new StringBuilder();
            Body.Append("<h3>Chào: <b>" + username + "<b></h3>");
            Body.Append("<p>Vào Lúc " + DateTime.Now.ToString() + " Bạn Chiến Thắng Thắng Trong Phiên Đấu Giá</p>");
            Body.Append("<table>");
            Body.Append("<tr><td colspan='2'><h4>Thông tin Sản Phẩm</h4></td></tr>");
            Body.Append("<tr><td>Tên Sản Phẩm: </td><td>" + pro.ProName + "</td></tr>");
            Body.Append("<tr><td>Giá Hiện Tại: </td><td>" + string.Format("{0:N0}", pro.AucPrice) + " VNĐ</td></tr>");
            Body.Append("</table>");
            return Body;
            //MailMessage mail = new MailMessage();
            //mail.To.Add("Dragonblue789@gmail.com");
            //mail.From = new MailAddress("admiweb2nhom5@gmail.com");
            //mail.Subject = "Thông Báo Đấu Giá Thành Công Sản Phẩm " + pro.ProName;
            //mail.Body = Body.ToString();// phần thân của mail ở trên
            //mail.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");// tài khoản Gmail của bạn
            //smtp.EnableSsl = true;
            //smtp.Send(mail);
        }
        public static StringBuilder GmailCoNgTraGia(Product pro, string username)
        {
            StringBuilder Body = new StringBuilder();
            Body.Append("<h3>Chào: <b>" + username + "<b></h3>");
            Body.Append("<p>Đã Có Người Trả Giá Lúc " + DateTime.Now.ToString() + "</p>");
            Body.Append("<p>Người Mua : " + Helper.StringUtils.Replace(pro.Owner) + " Đã Trả " + string.Format("{0:N0}", pro.AucPrice) + " VNĐ Cho Sản Phẩm " + pro.ProName + " Của Bạn</p>");
            Body.Append("<table>");
            Body.Append("<tr><td colspan='2'><h4>Thông tin Sản Phẩm</h4></td></tr>");
            Body.Append("<tr><td>Tên Sản Phẩm: </td><td>" + pro.ProName + "</td></tr>");
            Body.Append("<tr><td>Giá Hiện Tại: </td><td>" + string.Format("{0:N0}", pro.AucPrice) + " VNĐ</td></tr>");
            Body.Append("<tr><td>Thời Gian Kết Thúc Đấu Giá: </td><td>" + pro.EndTime.ToString() + "</td></tr>");
            Body.Append("</table>");
            return Body;
        }
        public static StringBuilder GmailBanTC(Product pro, string username)
        {
            StringBuilder Body = new StringBuilder();
            Body.Append("<h3>Chào: <b>" + username + "<b></h3>");
            Body.Append("<p>Phiên Đấu Giá Của Bạn Đã Kết Thúc Lúc " + DateTime.Now.ToString() + "</p>");
            Body.Append("<p>Người Mua : " + Helper.StringUtils.Replace(pro.Owner) + " Đã Chiến Thắng Trong Phiên Đấu Giá Của Bạn Với Số Tiền " + string.Format("{0:N0}", pro.AucPrice) + " VNĐ</p>");
            Body.Append("<table>");
            Body.Append("<tr><td colspan='2'><h4>Thông tin Sản Phẩm</h4></td></tr>");
            Body.Append("<tr><td>Tên Sản Phẩm: </td><td>" + pro.ProName + "</td></tr>");
            Body.Append("<tr><td>Giá Hiện Tại: </td><td>" + string.Format("{0:N0}", pro.AucPrice) + " VNĐ</td></tr>");
            Body.Append("</table>");
            return Body;
        }
        public static StringBuilder GmailBanTB(Product pro, string username)
        {
            StringBuilder Body = new StringBuilder();
            Body.Append("<h3>Chào: <b>" + username + "<b></h3>");
            Body.Append("<p>Phiên Đấu Giá Của Bạn Đã Kết Thúc Lúc " + DateTime.Now.ToString() + "</p>");
            Body.Append("<p>Rất Tiếc Không Ai Trả Giá Cho Sản Phẩm Của Bạn !!!</p>");
            Body.Append("<table>");
            Body.Append("<tr><td colspan='2'><h4>Thông tin Sản Phẩm</h4></td></tr>");
            Body.Append("<tr><td>Tên Sản Phẩm: </td><td>" + pro.ProName + "</td></tr>");
            Body.Append("<tr><td>Giá Hiện Tại: </td><td>" + string.Format("{0:N0}", pro.AucPrice) + " VNĐ</td></tr>");
            Body.Append("</table>");
            return Body;
        }

    }
    

}