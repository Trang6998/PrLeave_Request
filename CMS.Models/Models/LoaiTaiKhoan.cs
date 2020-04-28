using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("LoaiTaiKhoan")]
    public class LoaiTaiKhoan
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int LoaiTaiKhoanID { get; set; }
        public string TenLoai { get; set; }

        [InverseProperty("LoaiTaiKhoan")]
        public virtual ICollection<Users> Users { get; set; }
    }
}
