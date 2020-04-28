using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("CoSo")]
    public class CoSo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CoSoID { get; set; }
        [MaxLength(200)]
        public string TenCoSo { get; set; }
        public int? LoaiCoSo { get; set; }
        [MaxLength(200)]
        public string DiaChi { get; set; }

        [InverseProperty("CoSo")]
        public virtual ICollection<Users> Users { get; set; }

    }
}
