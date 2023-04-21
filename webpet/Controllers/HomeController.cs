using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webpet.Models;

namespace webpet.Controllers
{
    public class HomeController : Controller
    {
        dbPetDataContext data = new dbPetDataContext();
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.KeyWord = searchString;
            if (page == null) page = 1;
            var all_pet = (from Pet in data.Pets select Pet).OrderBy(m => m.mapet);
            if (!string.IsNullOrEmpty(searchString)) all_pet = (IOrderedQueryable<Pet>)all_pet.Where(a => a.ten.Contains(searchString));
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(all_pet.ToPagedList(pageNum, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}