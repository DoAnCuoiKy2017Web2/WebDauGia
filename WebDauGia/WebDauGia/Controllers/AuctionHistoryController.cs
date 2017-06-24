using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Models;

namespace WebDauGia.Controllers
{
    public class AuctionHistoryController : Controller
    {
        // GET: AuctionHistory
        public ActionResult Index()
        {
            return View();
        }
        //Get : AuctionHistory/Add
        public ActionResult Add()
        {
            return View();
        }
        //Post : AuctionHistory/UpdateBid
        [HttpPost]
        public ActionResult UpdateBid(double giatra, int proid)
        {
            var username = ((User)Session["user"]).UserName;

            using (var ctx = new QuanLyDauGiaEntities())
            {
                var pro = ctx.Products.Where(p => p.ProID == proid).FirstOrDefault();
                if (pro.EndTime <= DateTime.Now)
                {
                    return Json("Phiên Đấu Giá Đã Kết Thúc Lúc " + pro.EndTime.ToString(), JsonRequestBehavior.AllowGet);
                }
                //trường hợp thây đổi giá đề mua luôn sản phẩm
                if (giatra >= pro.Price && pro.Price!=0 && pro.Price!=null)
                {
                    string GmNgMua = (ctx.Users.Where(us => us.UserName == username).FirstOrDefault()).Email;
                    string GmNgBan = (ctx.Users.Where(us => us.UserName == pro.Salesman).FirstOrDefault()).Email;
                    //chiến thắng tuyệt đối
                    //lưu lịch sử
                    var auhis = new AuctionHistory();
                    auhis.ProID = proid;
                    auhis.UserName = username;
                    auhis.AucPrice = pro.Price;
                    auhis.Time = DateTime.Now;
                    ctx.AuctionHistorys.Add(auhis);
                    ctx.SaveChanges();
                    //update Product
                    pro.OwnerPrice = pro.Price;
                    pro.AucPrice = pro.Price;
                    pro.Owner = ((User)Session["user"]).UserName;
                    pro.EndTime = DateTime.Now;
                    ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                    //gửi gmail
                    //nguoi mua
                    MailMessage mail = new MailMessage();
                    mail.To.Add(GmNgMua);
                    mail.From = new MailAddress("admiweb2nhom5@gmail.com");
                    mail.Subject = "Thông Báo Thắng Cuộc Trong Phiên Đấu Giá Sản Phẩm " + pro.ProName;
                    mail.Body = Function.GmailTBChienThang(pro, username).ToString();
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    //gửi gmail cho người bán
                    MailMessage mail2 = new MailMessage();
                    mail2.To.Add(GmNgBan);
                    mail2.From = new MailAddress("admiweb2nhom5@gmail.com");
                    mail2.Subject = "Thông Báo Đã Có Người Mua Thành Công Sản Phẩm Của Bạn " + pro.ProName;
                    mail2.Body = Function.GmailBanTC(pro, pro.Salesman).ToString();
                    mail2.IsBodyHtml = true;
                    SmtpClient smtp2 = new SmtpClient();
                    smtp2.Host = "smtp.gmail.com";
                    smtp2.Port = 587;
                    smtp2.UseDefaultCredentials = true;
                    smtp2.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                    smtp2.EnableSsl = true;
                    smtp2.Send(mail2);
                    return Json("Chúc Mừng Bạn Đã Chiến Thắng Trong Phiên Đấu Giá Này Với Số Tiền" + string.Format("{0:N0}", pro.Price) + "VNĐ", JsonRequestBehavior.AllowGet);
                }
                //còn là người giữ giá
                if (pro.Owner == username)
                {
                    if (giatra < pro.AucPrice)
                    {
                        return Json("Đổi Giá Thất Bại! Giá Hiện Tại Đã Bị Thay Đổi", JsonRequestBehavior.AllowGet);
                    }
                    else if (giatra >= (pro.AucPrice + pro.StepPrice))
                    {
                        pro.OwnerPrice = giatra;
                        ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        return Json("Đổi Giá Thành Công!", JsonRequestBehavior.AllowGet);
                    }
                }
                //đã mất quyền giữ giá không thể đổi giá
                else
                {
                    return Json("Đổi Giá Thất Bại! Đã Có Người Khác Giữ Giá Sản Phẩm Này!", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Hệ Thống Quá Tải. Vui Lòng Thử Lại!", JsonRequestBehavior.AllowGet);
        }
        //Post : AuctionHistory/Add
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Add(double giatra, int proid)
        {
            int TH = 0;
            var username = ((User)Session["user"]).UserName;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                var pro = ctx.Products.Where(p => p.ProID == proid).FirstOrDefault();
                //luu tên người giữ giá trước.
                string nameold = pro.Owner;
                if (pro.EndTime <= DateTime.Now)
                {
                    return Json("Phiên Đấu Giá Đã Kết Thúc Lúc " + pro.EndTime.ToString(), JsonRequestBehavior.AllowGet);
                }
                if (giatra < (pro.AucPrice + pro.StepPrice))
                {
                    return Json("Đã Có Người Trả Giá Cao Hơn Bạn! Giá Hiện Tại Là " + string.Format("{0:N0}", pro.AucPrice), JsonRequestBehavior.AllowGet);
                }
                string GmNgMua = (ctx.Users.Where(us => us.UserName == username).FirstOrDefault()).Email;
                string GmNgBan = (ctx.Users.Where(us => us.UserName == pro.Salesman).FirstOrDefault()).Email;
                string GmNgMuaCu = "";
                if (pro.Owner != null)
                {
                    GmNgMuaCu = (ctx.Users.Where(us => us.UserName == pro.Owner).FirstOrDefault()).Email;
                }
                //tang lượt đâu giá lên 1
                pro.NumOfAuction += 1;
                //van con giữ giá
                if (giatra >= pro.Price && pro.Price!=0 && pro.Price!=null)
                {
                    TH = 3;//chiến thắng tuyệt đối
                    //update Product
                    pro.OwnerPrice = pro.Price;
                    pro.AucPrice = pro.Price;
                    pro.Owner = ((User)Session["user"]).UserName;
                    pro.EndTime = DateTime.Now;
                    ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();

                }
                else if (giatra > pro.OwnerPrice  || pro.OwnerPrice ==null)
                {
                    TH = 1;// 1 là chiến thắng trờ thành người giữ giá
                    int RestTime = Function.Rest(pro.EndTime);
                    if (0 < RestTime && RestTime <= 5 * 60 && pro.AutoRenewal==true)
                    {
                        pro.EndTime = pro.EndTime.AddMinutes(10);
                    }
                    //update Product
                    if (pro.Owner==null)
                    {
                        pro.AucPrice += pro.StepPrice;
                    }
                    else
                    {
                        pro.AucPrice = (double)pro.OwnerPrice + pro.StepPrice;
                    }
                    pro.OwnerPrice = giatra;
                    pro.Owner = ((User)Session["user"]).UserName;
                    ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                else if (giatra <= pro.OwnerPrice)
                {
                    TH = 2;//2 la thua 
                    //update Product
                    pro.AucPrice = giatra;
                    ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                if (TH == 1 || TH == 2 || TH == 3)
                {
                    var auhis = new AuctionHistory();
                    auhis.ProID = proid;
                    auhis.UserName = username;
                    if (TH == 3)
                    { auhis.AucPrice = pro.Price; }
                    else { auhis.AucPrice = (double)pro.AucPrice; }
                    auhis.Time = DateTime.Now;
                    ctx.AuctionHistorys.Add(auhis);
                    ctx.SaveChanges();
                }
                try
                {
                    if (TH == 3)
                    {
                        //gửi gmail
                        //nguoi mua
                        MailMessage mail = new MailMessage();
                        mail.To.Add(GmNgMua);
                        mail.From = new MailAddress("admiweb2nhom5@gmail.com");
                        mail.Subject = "Thông Báo Thắng Cuộc Trong Phiên Đấu Giá Sản Phẩm " + pro.ProName;
                        mail.Body = Function.GmailTBChienThang(pro, username).ToString();
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        //gửi gmail cho người thua nếu có
                        if (nameold != null)
                        {
                            if (nameold != ((User)Session["user"]).UserName)
                            {
                                MailMessage mail1 = new MailMessage();
                                mail1.To.Add(GmNgMuaCu);
                                mail1.From = new MailAddress("admiweb2nhom5@gmail.com");
                                mail1.Subject = "Thông Báo Bị Cướp Quyền Giá Sản Phẩm " + pro.ProName;
                                mail1.Body = Function.GmailMatQuyenGiuGia(pro, nameold).ToString();
                                mail1.IsBodyHtml = true;
                                SmtpClient smtp1 = new SmtpClient();
                                smtp1.Host = "smtp.gmail.com";
                                smtp1.Port = 587;
                                smtp1.UseDefaultCredentials = true;
                                smtp1.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                                smtp1.EnableSsl = true;
                                smtp1.Send(mail1);
                            }
                        }
                        //gửi gmail cho người bán
                        MailMessage mail2 = new MailMessage();
                        mail2.To.Add(GmNgBan);
                        mail2.From = new MailAddress("admiweb2nhom5@gmail.com");
                        mail2.Subject = "Thông Báo Đã Có Người Mua Thành Công Sản Phẩm Của Bạn " + pro.ProName;
                        mail2.Body = Function.GmailBanTC(pro, pro.Salesman).ToString();
                        mail2.IsBodyHtml = true;
                        SmtpClient smtp2 = new SmtpClient();
                        smtp2.Host = "smtp.gmail.com";
                        smtp2.Port = 587;
                        smtp2.UseDefaultCredentials = true;
                        smtp2.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                        smtp2.EnableSsl = true;
                        smtp2.Send(mail2);
                        //
                        pro.Status = true;
                        ctx.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                    }
                    if (TH == 1)
                    {
                        //gửi mail
                        //nguoi mua
                        MailMessage mail = new MailMessage();
                        mail.To.Add(GmNgMua);
                        mail.From = new MailAddress("admiweb2nhom5@gmail.com");
                        mail.Subject = "Thông Báo Bạn Trở Thành Người Giữ Giá Sản Phẩm " + pro.ProName;
                        mail.Body = Function.GmailDuocQuyenGiuGia(pro, username).ToString();
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        //gửi gmail cho người Mất Quyền Giữ Giá
                        if (nameold != null)
                        {
                            if (nameold != ((User)Session["user"]).UserName)
                            {
                                MailMessage mail1 = new MailMessage();
                                mail1.To.Add(GmNgMuaCu);
                                mail1.From = new MailAddress("admiweb2nhom5@gmail.com");
                                mail1.Subject = "Thông Báo Bị Cướp Quyền Giá Sản Phẩm " + pro.ProName;
                                mail1.Body = Function.GmailMatQuyenGiuGia(pro, nameold).ToString();
                                mail1.IsBodyHtml = true;
                                SmtpClient smtp1 = new SmtpClient();
                                smtp1.Host = "smtp.gmail.com";
                                smtp1.Port = 587;
                                smtp1.UseDefaultCredentials = true;
                                smtp1.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                                smtp1.EnableSsl = true;
                                smtp1.Send(mail1);
                            }
                        }
                        //gửi gmail cho người bán
                        MailMessage mail2 = new MailMessage();
                        mail2.To.Add(GmNgBan);
                        mail2.From = new MailAddress("admiweb2nhom5@gmail.com");
                        mail2.Subject = "Thông Báo Có Người Ra Giá Cho Sản Phẩm " + pro.ProName + " Của Bạn";
                        mail2.Body = Function.GmailCoNgTraGia(pro, pro.Salesman).ToString();
                        mail2.IsBodyHtml = true;
                        SmtpClient smtp2 = new SmtpClient();
                        smtp2.Host = "smtp.gmail.com";
                        smtp2.Port = 587;
                        smtp2.UseDefaultCredentials = true;
                        smtp2.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                        smtp2.EnableSsl = true;
                        smtp2.Send(mail2);
                    }
                }
                catch
                {
                    return Json("Thao Tác Ra Giá Của Bạn Đã Thành Công! Chúng Tôi Đang Gập Rắc Rối Về Việc Gửi Email Cho Quý Khách! Xin Lỗi Vì Sự Bất Tiện Này", JsonRequestBehavior.AllowGet);
                }
            }
            //return RedirectToAction("Detail", "Product", new { ID = a.ProID });
            return Json(TH, JsonRequestBehavior.AllowGet);
        }


        ////Post : AuctionHistory/Add
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Add(AuctionHistory model)
        //{
        //    using (var ctx = new QuanLyDauGiaEntities())
        //    {
        //        ctx.Entry(model).State = System.Data.Entity.EntityState.Added;
        //        ctx.SaveChanges();
        //        @ViewBag.Message = "Đã thêm thành công.";
        //    }
        //    return View();
        //}

        //Get : AuctionHistory/Edit
        public ActionResult Edit(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "AuctionHistory");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                AuctionHistory auch = dt.AuctionHistorys
                    .Where(p => p.AucID == ID)
                    .FirstOrDefault();
                if (auch != null)
                {
                    return View(auch);
                }
                return RedirectToAction("Index", "AuctionHistory");
            }
        }
        //post: AuctionHistory/Show
        [HttpPost]
        public ActionResult Show(int proid)
        {
            List<AuctionHistory> hisau;
            using (var ctx= new QuanLyDauGiaEntities())
            {
                hisau = ctx.AuctionHistorys.Where(his => his.ProID == proid).OrderByDescending(his=>his.Time).ToList();
            }
            return Json(hisau, JsonRequestBehavior.AllowGet);
        }
        //Post: AuctionHistory/Update
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(AuctionHistory model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                @ViewBag.Message = "Cập nhật thành công.";
            }
            return RedirectToAction("Edit", "AuctionHistory", new { ID = model.AucID });
        }

        //Get : AuctionHistory/Delete
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "AuctionHistory");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                AuctionHistory auch = dt.AuctionHistorys
                    .Where(p => p.ProID == ID)
                    .FirstOrDefault();
                return View(auch);
            }
        }

        //Post : AuctionHistory/Deleted
        [HttpPost]
        public ActionResult Deleted(int ID)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                AuctionHistory auch = ctx.AuctionHistorys
                    .Where(p => p.AucID == ID)
                    .FirstOrDefault();
                ctx.Entry(auch).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "AuctionHistory");
        }
    }
}