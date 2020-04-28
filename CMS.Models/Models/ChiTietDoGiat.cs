using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("ChiTietDoGiat")]
    public class ChiTietDoGiat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ChiTietDoGiatID { get; set; }

        public int? KhachDatHangID { get; set; }

        public int? DoGiatID { get; set; }

        public int? HinhThucGiatID { get; set; }

        public int? SoLuong { get; set; }

        [ForeignKey("DoGiatID")]
        public virtual DoGiat DoGiat { get; set; }

        [ForeignKey("KhachDatHangID")]
        public virtual KhachDatHang KhachDatHang { get; set; }
        [ForeignKey("HinhThucGiatID")]
        public virtual HinhThucGiat HinhThucGiat { get; set; }

    }
}
