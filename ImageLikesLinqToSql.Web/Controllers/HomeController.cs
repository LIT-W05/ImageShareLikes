using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageLikesLinqToSql.Data;
using ImageLikesLinqToSql.Web.Models;

namespace ImageLikesLinqToSql.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var repo = new ImagesRepository(Properties.Settings.Default.ConStr);
            return View(repo.GetImages());
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(string title, HttpPostedFileBase imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            imageFile.SaveAs(Path.Combine(Server.MapPath("/UploadedImages"), fileName));
            var image = new Image
            {
                Title = title,
                FileName = fileName,
                DateUploaded = DateTime.Now
            };
            var db = new ImagesRepository(Properties.Settings.Default.ConStr);
            db.Add(image);
            return RedirectToAction("Index");
        }

        public ActionResult ViewImage(int id)
        {
            var db = new ImagesRepository(Properties.Settings.Default.ConStr);
            var image = db.GetImage(id);
            if (image == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new ViewImageViewModel
            {
                Image = image
            };
            if (Session["likedids"] != null)
            {
                var likedIds = (List<int>)Session["likedids"];
                //vm.CanLikeImage = !likedIds.Any(i => i == id);
                vm.CanLikeImage = likedIds.All(i => i != id);
            }
            else
            {
                vm.CanLikeImage = true;
            }
            return View(vm);
        }

        [HttpPost]
        public void LikeImage(int id)
        {
            var db = new ImagesRepository(Properties.Settings.Default.ConStr);
            db.AddLike(id);
            List<int> likedIds;
            if (Session["likedids"] != null)
            {
                likedIds = (List<int>)Session["likedids"];
            }
            else
            {
                likedIds = new List<int>();
                Session["likedids"] = likedIds;
            }

            likedIds.Add(id);
        }

        public ActionResult GetLikes(int id)
        {
            var db = new ImagesRepository(Properties.Settings.Default.ConStr);
            return Json(new { Likes = db.GetLikes(id) }, JsonRequestBehavior.AllowGet);
        }
    }
}