using NavtechAPIServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace NavtechAPIServices.Controllers
{
    public class CustomersController : ApiController
    {
        NavtechCustomerServices _customerServiceObj = new NavtechCustomerServices();
        public CustomersController()
        {
        }
        private NavtechEF db = new NavtechEF();

        /// <summary>
        /// Method to add new Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        // POST: api/Customers/AddNewCustomer
        [ResponseType(typeof(Customer))]
        public HttpResponseMessage AddNewCustomer(Customer customer)
        {
            try
            {
                if (!ModelState.IsValid) //if given values are not valid
                {
                   return Request.CreateResponse(HttpStatusCode.OK, new { CustomerId = 0 , Message = "Please enter valid customer details" });
                }
                else
                {
                    if (_customerServiceObj.EmailsExists(customer.EmailId)) //if emailid already exists
                        return Request.CreateResponse(HttpStatusCode.OK, new { CustomerId = 0, Message = "Email already exists" });
                    else
                    {
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new { CustomerId = customer.CustomerId, Message = "Customer added successfully" });
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { CustomerId = 0, Message = ex.Message});
            }
        }

        //Method to get Customer orders with Items
        [HttpGet]
        public HttpResponseMessage CustomerOrdersList(long customerId,int index,int pageSize)
        {
            var CustomerOrders = _customerServiceObj.GetCustomerOrders(customerId).ToList();
            var skipValue = (index-1) * pageSize;
            List<CustomerViewModel> res = CustomerOrders.Skip(skipValue).Take(pageSize).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                CustomerOrders = res
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(long id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }
    }
}