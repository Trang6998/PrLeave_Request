using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("LoaiDoGiat")]
    public class LoaiDoGiat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int LoaiDoGiatID { get; set; }

        [MaxLength(50)]
        public string TenLoaiDoGiat { get; set; }

        public string MoTa { get; set; }

        [InverseProperty("LoaiDoGiat")]
        public virtual ICollection<DoGiat> DoGiat { get; set; }

    }
}
