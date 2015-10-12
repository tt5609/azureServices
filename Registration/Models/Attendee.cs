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
        [Required(ErrorMessage = "Name is required 請告訴我們您的大名")]
        [Display(Name = "Name 姓名:")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required 請告訴我們您的電子郵件以方便連絡")]
        [Display(Name = "Email 電子郵件地址:")]
        [EmailAddress(ErrorMessage = "Invalid Email Address 請提供正確的電子郵件地址")]
        public string Email { get; set; }
        [Range(1, 100)]
        [Required(ErrorMessage = "Please provide the number of attendees 請告訴我們有多少位參加")]
        [Display(Name = "Number of people 人數:")]
        public Int32 NumberOfAttendee { get; set; }
        [Display(Name = "Would you like to join the banquet 是否參加晚宴?")]
        public bool AttendBanquet { get; set; }
        [Display(Name="Meal Type")]
        public string MealType { get; set; }
        [Display(Name = "Special Note 特別需求(例如幾份素食, 幾份一般餐點): ")]
        public string Comment { get; set; }

        public DateTime LastUpdated { get; set; }

        public List<Meal> MealTypes { get; set; }
    }

    public class Meal
    {
		public int Id
		{
			get;
			set;
		}
        public string Name { get; set; }
		public MealType Type
		{
			get;
			set;
		}
        public IEnumerable<string> Descriptions { get; set; }
    }

	public enum MealType
	{
		Regular,
		Vegetarian
	}
}