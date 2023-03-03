using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_COMMERCE.Models
{
    public class Ent_Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description{ get; set; }
        public byte[] image { get; set; }
        public string status { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string lastmodifiedBy { get; set; }
        public string lastModifiedDate { get; set; }

    }
}