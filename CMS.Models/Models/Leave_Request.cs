using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("Leave_Request")]
    public class Leave_Request
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        public int? User_LeaveID { get; set; }

        public int? User_ApproveID { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public string Reason { get; set; }

        [ForeignKey("User_LeaveID")]
        public virtual Users User_Leave { get; set; }
        [ForeignKey("User_ApproveID")]
        public virtual Users User_Approve { get; set; }

    }
}
