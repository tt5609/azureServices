using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class Attendee
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name 姓名")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email 電子郵件地址")]
        public string Email { get; set; }
        [Display(Name = "Number of people 人數")]
        public Int32 NumberOfAttendee { get; set; }
        [Display(Name = "Would you like to join the banque? 是否參加晚宴?")]
        public bool AttendBanquet { get; set; }
        [Display(Name = "Special Note: ")]
        public string Comment { get; set; }
    }
}