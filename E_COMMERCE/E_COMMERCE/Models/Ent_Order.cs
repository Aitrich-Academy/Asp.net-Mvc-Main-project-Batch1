using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECOMMERSE.Models
{
    public class Ent_Order
    {
        public int userid { get; set; }
        public int productid { get; set; }
        public int quantity { get; set; }
        public int total { get; set; }
        public int status { get; set; }
        public string modifydate { get; set; }
    }
}