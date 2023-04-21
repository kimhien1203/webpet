using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webpet.Models;

namespace webpet.Controllers
{
    public class PetController : Controller
    {
        dbPetDataContext data = new dbPetDataContext();

        // GET: Pet
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.KeyWord = searchString;
            if (page == null) page = 1;
            var all_pet = (from Pet in data.Pets select Pet).OrderBy(m => m.mapet);
            if (!string.IsNullOrEmpty(searchString)) all_pet = (IOrderedQueryable<Pet>)all_pet.Where(a => a.ten.Contains(searchString));
            int pageSize = 3;
            int pageNum = page ?? 1;
            return View(all_pet.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Detail(int id)
        {
            var D_pet = data.Pets.Where(m => m.mapet == id).First();
            return View(D_pet);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Pet p)
        {
            var E_tenpet = collection["ten"];
            var E_xuatsu = collection["xuatsu"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_soluong = Convert.ToInt32(collection["soluong"]);
            var E_hinh = collection["hinh"];
            if (string.IsNullOrEmpty(E_tenpet))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                p.ten = E_tenpet.ToString();
                p.xuatsu = E_xuatsu.ToString();
                p.giaban = E_giaban;
                p.ngaycapnhat = E_ngaycapnhat;
                p.soluong= E_soluong;
                p.hinh = E_hinh.ToString();
                data.Pets.InsertOnSubmit(p);
                data.SubmitChanges();

                return RedirectToAction("Index");
            }
            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var E_pet = data.Pets.First(m => m.mapet == id);
            return View(E_pet);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_pet = data.Pets.First(m => m.mapet == id);
            var E_tenpet = collection["ten"];
            var E_xuatsu = collection["xuatsu"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_soluong = Convert.ToInt32(collection["soluong"]);
            var E_hinh = collection["hinh"];
            E_pet.mapet = id;
            if (string.IsNullOrEmpty(E_tenpet))
            {
                ViewData["Error"] = "don't empty";
            }
            else
            {
                E_pet.ten = E_tenpet.ToString();
                E_pet.xuatsu = E_xuatsu.ToString();
                E_pet.hinh = E_hinh.ToString();
                E_pet.giaban = E_giaban;
                E_pet.ngaycapnhat = E_ngaycapnhat;
                E_pet.soluong = E_soluong;
                UpdateModel(E_pet);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
        public ActionResult Delete(int id)
        {
            var D_Pet = data.Pets.First(m => m.mapet == id);
            return View(D_Pet);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_Pet = data.Pets.Where(m => m.mapet == id).First();
            data.Pets.DeleteOnSubmit(D_Pet);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
      

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        public ActionResult ConHang()
        {
            return View(data.Pets.Where(x => x.soluong > 0).ToList());
        }
        public ActionResult SapMoBan()
        {
            return View(data.Pets.Where(x => x.ngaymoban.Value.Date > DateTime.Now.Date).ToList());
        }
    }
}