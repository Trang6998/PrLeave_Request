using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("KhachDatHang")]
    public class KhachDatHang
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int KhachDatHangID { get; set; }

        public int? NguoiDungID { get; set; }
        public int? UserID { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? NgayDat { get; set; }

        public int? GioDat { get; set; }

        public string GhiChu { get; set; }

        public int? ThanhTien { get; set; }
        public int? TinhTrangXuLy { get; set; }
        public int? HinhThucThanhToan { get; set; }
        public int? TinhTrangThanhToan { get; set; }

        [ForeignKey("NguoiDungID")]
        public virtual NguoiDung NguoiDung { get; set; }
        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }

        [InverseProperty("KhachDatHang")]
        public virtual ICollection<ChiTietDoGiat> ChiTietDoGiat { get; set; }

    }
}
