using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webpet.Models;

namespace webpet.Controllers
{
    public class GioHangController : Controller
    {
        dbPetDataContext data = new dbPetDataContext();

        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;

            if (listGiohang == null)
            {
                listGiohang = new List<GioHang>();

                Session["GioHang"] = listGiohang;
            }
            return listGiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> listGiohang = LayGioHang();

            GioHang sanpham = listGiohang.Find(n => n.mapet == id);

            if (sanpham == null)
            {
                sanpham = new GioHang(id);

                listGiohang.Add(sanpham);

                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;

                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;

            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;

            if (listGiohang != null)
            {
                tsl = listGiohang.Sum(n => n.iSoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;

            if (listGiohang != null)
            {
                tsl = listGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;

            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;

            if (listGiohang != null)
            {
                tt = listGiohang.Sum(n => n.dThanhtien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<GioHang> listGiohang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();

            return View(listGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();

            return PartialView();
        }
        public ActionResult XoaGiohang(int id)
        {
            List<GioHang> listGiohang = LayGioHang();

            GioHang sanpham = listGiohang.SingleOrDefault(n => n.mapet == id);

            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.mapet == id);

                return RedirectToAction("Giohang");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<GioHang> listGiohang = LayGioHang();

            GioHang sanpham = listGiohang.SingleOrDefault(n => n.mapet == id);

            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["txtSolg"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> listGiohang = LayGioHang();

            listGiohang.Clear();

            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Pet");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Pet p = new Pet();

            List<GioHang> gh = LayGioHang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;

            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.mapet = item.mapet;
                ctdh.soluong = item.iSoluong;
                ctdh.gia = (decimal)item.gianban;
                p = data.Pets.Single(n => n.mapet == item.mapet);
                p.soluong = ctdh.soluong;
                data.SubmitChanges();

                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonhang", "Giohang");
        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }
    }
}