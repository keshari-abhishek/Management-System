using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementSystem.Models
{
    //replica for student table used for student registration form
    public class Student
    {
        public int s_id { get; set; }

        [Display(Name ="Student Name")]
        [Required(ErrorMessage ="Please Enter Your Name")]
        public string s_name { get; set; }

        [Display(Name = "Student Phone Number")]
        public string s_phone { get; set; }

        [Display(Name = "Student Mail Id")]
        [Required(ErrorMessage ="Please Enter Your Email Id")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Invalid Email id")]
       // [Remote("IsEamilAllreadyRegistered", "Home",HttpMethod="Post",ErrorMessage ="Email address all ready registered")]
        public string s_email { get; set; }

        [Display(Name = "Chose Your Password")]
        public string s_password { get; set; }

        [Display(Name = "Select Your Branch")]
        public string s_branch_code { get; set; }

        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public DateTime deletedOn { get; set; }
        public bool isdeleted { get; set; }
        public bool active { get; set; }
        public string guid { get; set; }
    }
}