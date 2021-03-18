using NavtechAPIServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using NavtechAPIServices;
using MoreLinq;

namespace NavtechAPIServices
{
    public class NavtechCustomerServices
    {
        private NavtechEF db = new NavtechEF();
        public NavtechCustomerServices()
        {
        }
        //Method to find whether email id exists or not
        public bool EmailsExists(string emailId)
        {
            bool isExists = db.Customers.Where(x => x.EmailId == emailId).Any();
            return isExists;
        }
        //method to find whether customer exists or not
        public bool IsCustomerExists(long customerId)
        {
            bool isExists = db.Customers.Where(x => x.CustomerId == customerId).Any();
            return isExists;
        }

        public List<CustomerViewModel> GetCustomerOrders(long customerId)
        {
            var list = (from o in db.Orders.Where(x => x.CustomerId == customerId).ToList()
                        join c in db.Customers.Where(x => x.CustomerId == customerId).ToList()
                        on o.CustomerId equals c.CustomerId 
                        join oi in db.OrderItems
                        on o.OrderId equals oi.OrderId 
                        join i in db.Items
                        on oi.ItemId equals i.ItemId
                        select new CustomerViewModel
                        {
                            CustomerName = c.FirstName,
                            ShippingAddress = c.ShippingAddress,
                            ItemName = i.ItemName,
                            OrderId = o.OrderId,
                            Quantity = oi.Quantity,
                            ItemId = oi.ItemId
                        }).DistinctBy(x=>x.ItemId).ToList();

            return list;
        }
    }
}