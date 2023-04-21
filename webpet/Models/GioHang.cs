using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace webpet.Models
{
    public class GioHang
    {
        dbPetDataContext data = new dbPetDataContext();
        public int mapet { get; set; }
        [Display(Name = "Tên pet")]
        public string ten { get; set; }
        [Display(Name = "Ảnh bìa")]
        public string hinh { get; set; }
       

        [Display(Name = "Giá bán")]
        public Double gianban { get; set; }
        [Display(Name = "Số lượng")]
        public int iSoluong { get; set; }

        [Display(Name = "Thành tiền")]
        public Double dThanhtien
        {
            get { return iSoluong * gianban; }
        }

        public GioHang(int id)
        {
            mapet = id;
            Pet pet = data.Pets.Single(n => n.mapet == mapet);
            ten = pet.ten;
            hinh = pet.hinh;
           
            gianban = double.Parse(pet.giaban.ToString());
            iSoluong = 1;
        }

    }
}