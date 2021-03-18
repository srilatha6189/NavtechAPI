using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NavtechAPIServices.Models
{
    public class CustomerViewModel
    {
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ShippingAddress { get; set; }
        public long OrderId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public long ItemId { get; set; }
    }
}