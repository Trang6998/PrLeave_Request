using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("LuotTruyCap")]
    public class LuotTruyCap
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int LuotTruyCapId { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(20)]
        public string IP { get; set; }

        [Required]
        public DateTime ThoiGian { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(250)]
        public string ThietBi { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(250)]
        public string TrangTruyCapDauTien { get; set; }

    }
}
