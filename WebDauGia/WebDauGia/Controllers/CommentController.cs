using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Models;

namespace WebDauGia.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        //Get : Comment/Add
        public ActionResult Add()
        {
            return View();
        }

        //Post : Comment/Add
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Comment model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Added;
                ctx.SaveChanges();
                @ViewBag.Message = "Đã thêm thành công.";
            }
            return View();
        }

        //Get : Comment/Edit
        public ActionResult Edit(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Comment");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                Comment cmt = dt.Comments
                    .Where(p => p.CmtID == ID)
                    .FirstOrDefault();
                if (cmt != null)
                {
                    return View(cmt);
                }
                return RedirectToAction("Index", "Comment");
            }
        }

        //Post: Comment/Update
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Comment model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                @ViewBag.Message = "Cập nhật thành công.";
            }
            return RedirectToAction("Edit", "Comment", new { ID = model.CmtID });
        }

        //Get : Comment/Delete
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Comment");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                Comment cmt = dt.Comments
                    .Where(p => p.CmtID == ID)
                    .FirstOrDefault();
                return View(cmt);
            }
        }

        //Post : Comment/Deleted
        [HttpPost]
        public ActionResult Deleted(int ID)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                Comment cmt = ctx.Comments
                    .Where(p => p.CmtID == ID)
                    .FirstOrDefault();
                ctx.Entry(cmt).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "Comment");
        }
    }
}