using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class SubmittedAssignment
    {
        public int a_id { get; set; }
        public int s_id { get; set; }
        public int f_id { get; set; }
        public string path { get; set; }
        public string remarks { get; set; }
        public int marks { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime deletedOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public bool isDeleted { get; set; }

    }
}