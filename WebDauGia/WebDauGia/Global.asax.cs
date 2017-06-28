using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebDauGia.Models;

namespace WebDauGia
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //System.Timers.Timer aTimer = new System.Timers.Timer(10 * 1000);
            //aTimer.Elapsed += new System.Timers.ElapsedEventHandler(EventGuiMail);
            //aTimer.AutoReset = true;
            //aTimer.Enabled = true;
            //aTimer.Start();
        }
        private static void EventGuiMail(object source, System.Timers.ElapsedEventArgs e)
        {
            //Send Email;
            List<Product> index;
            using (QuanLyDauGiaEntities ctx = new QuanLyDauGiaEntities())
            {
                var pro = ctx.Products.Where(p => p.EndTime <= DateTime.Now && (p.Status == false || p.Status == null)).ToList();
                index = pro;
                if (pro.Count > 0)
                {
                    for (int i = 0; i < pro.Count; i++)
                    {
                        try
                        {

                            if (pro[i].Owner == null)
                            {
                                string use = pro[i].Salesman;
                                string GmNgBan = (ctx.Users.Where(us => us.UserName == use).FirstOrDefault()).Email;
                                //viet gmail Bán Thất Bại
                                MailMessage mail2 = new MailMessage();
                                mail2.To.Add(GmNgBan);
                                mail2.From = new MailAddress(Email.DCMail);
                                mail2.Subject = "Thông Báo Sản Phẩm Của Bạn Không Có Người Đâu Giá " + pro[i].ProName;
                                mail2.Body = Function.GmailBanTB(pro[i], pro[i].Salesman).ToString();
                                mail2.IsBodyHtml = true;
                                SmtpClient smtp2 = new SmtpClient();
                                smtp2.Host = "smtp.gmail.com";
                                smtp2.Port = 587;
                                smtp2.UseDefaultCredentials = true;
                                smtp2.Credentials = new System.Net.NetworkCredential(Email.DCMail, Email.Pass);
                                smtp2.EnableSsl = true;
                                smtp2.Send(mail2);
                                pro[i].Status = true;
                                ctx.Entry(pro[i]).State = System.Data.Entity.EntityState.Modified;
                                ctx.SaveChanges();
                            }
                            else if (pro[i].Owner != null)
                            {
                                string useNM = pro[i].Owner;
                                string useNB = pro[i].Salesman;
                                string GmNgMua = (ctx.Users.Where(us => us.UserName == useNM).FirstOrDefault()).Email;
                                string GmNgBan = (ctx.Users.Where(us => us.UserName == useNB).FirstOrDefault()).Email;
                                //gui ngươi ban
                                MailMessage mail2 = new MailMessage();
                                mail2.To.Add(GmNgBan);
                                mail2.From = new MailAddress(Email.DCMail);
                                mail2.Subject = "Thông Báo Đã Có Người Mua Thành Công Sản Phẩm Của Bạn " + pro[i].ProName;
                                mail2.Body = Function.GmailBanTC(pro[i], pro[i].Salesman).ToString();
                                mail2.IsBodyHtml = true;
                                SmtpClient smtp2 = new SmtpClient();
                                smtp2.Host = "smtp.gmail.com";
                                smtp2.Port = 587;
                                smtp2.UseDefaultCredentials = true;
                                smtp2.Credentials = new System.Net.NetworkCredential(Email.DCMail, Email.Pass);
                                smtp2.EnableSsl = true;
                                smtp2.Send(mail2);
                                //gui nguoi mua
                                MailMessage mail = new MailMessage();
                                mail.To.Add(GmNgMua);
                                mail.From = new MailAddress(Email.DCMail);
                                mail.Subject = "Thông Báo Thắng Cuộc Trong Phiên Đấu Giá Sản Phẩm " + pro[i].ProName;
                                mail.Body = Function.GmailTBChienThang(pro[i], pro[i].Owner).ToString();
                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.Port = 587;
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = new System.Net.NetworkCredential(Email.DCMail, Email.Pass);
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                                //
                                pro[i].Status = true;
                                ctx.Entry(pro[i]).State = System.Data.Entity.EntityState.Modified;
                                ctx.SaveChanges();
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

    }
}
