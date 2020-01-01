using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class Assignement
    {
        public int a_id { get; set; }
        public int f_id { get; set; }
        public string a_name { get; set; }
        public string path { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime deletedOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public bool isdeleted {get;set;}
    }
}