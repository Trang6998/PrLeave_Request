using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int NguoiDungID { get; set; }

        [MaxLength(100)]
        public string TenNguoiDung { get; set; }

        [MaxLength(15)]
        public string SoDienThoai { get; set; }

        [MaxLength(15)]
        public string SoKhac { get; set; }

        [MaxLength(150)]
        public string DiaChi { get; set; }

        [MaxLength(50)]
        public string TaiKhoan { get; set; }

        [MaxLength(50)]
        public string Facebook { get; set; }

        [MaxLength(100)]
        public string Gmail { get; set; }
        public int? DiemThuong { get; set; }

        [InverseProperty("NguoiDung")]
        public virtual ICollection<KhachDatHang> KhachDatHang { get; set; }

    }
}
