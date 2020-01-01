using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class ExceptionReplica
    {
        public int ex_id { get; set; }
        public int exceptionNumber { get; set; }
        public string message { get; set; }
        public string method { get; set; }
        public string url { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime deletedOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public bool isdeleted { get; set; }
    }
}