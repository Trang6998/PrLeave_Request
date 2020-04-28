using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("AccessTokens", Schema ="auth"),]
    public class AccessTokens
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int AccessTokenId { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(200)]
        public string Token { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        public DateTimeOffset EffectiveTime { get; set; }

        [Required]
        public int ExpiresIn { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(200)]
        public string UserAgent { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(20)]
        public string IP { get; set; }

    }
}
