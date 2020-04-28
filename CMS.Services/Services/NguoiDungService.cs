using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services
{   
    public class NguoiDungService : ServiceBase<NguoiDung>, INguoiDungService
    {
        public bool ThemNguoiDung(NguoiDung nguoiDung)
        {
            NguoiDung nguoiDungCheck = DbContext.NguoiDung.SingleOrDefault(x => x.SoDienThoai == nguoiDung.SoDienThoai);
            if (nguoiDungCheck == null)
            {
                DbContext.NguoiDung.Add(nguoiDung);
            }
            else
            {
                nguoiDungCheck.DiaChi = nguoiDung.DiaChi;
                nguoiDungCheck.TenNguoiDung = nguoiDung.TenNguoiDung;
                nguoiDungCheck.SoKhac = nguoiDung.SoKhac;
                nguoiDungCheck.Gmail = nguoiDung.Gmail;
            }
            try
            {
                DbContext.SaveChanges();
                return true;
            } catch(Exception e)
            {
                return false;
            }
        }
    }
}
