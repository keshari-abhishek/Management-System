using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    //replica for faculty table used for faculty registration form
    public class Faculty
    {
        public int f_id { get; set; }
        [Display(Name ="Enter the faculty Name")]
        public string f_name { get; set; }
        [Display(Name = "Enter the Phone Number")]
        public string f_phone { get; set; }
        [Display(Name = "Enter the Email Id")]
        public string f_email { get; set; }
        [Display(Name = "Chosse Your Password")]
        public string f_password { get; set; }
        [Display(Name = "Select Your Subject Cobe")]
        public string f_subject_code { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public DateTime deletedOn { get; set; }
        public bool isdeleted { get; set; }
        public bool active { get; set; }
        public string guid { get; set; }
    }
}