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
using NavtechAPIServices;


namespace NavtechAPIServices.Controllers
{
    public class OrdersController : ApiController
    {
        private NavtechEF db = new NavtechEF();
        NavtechCustomerServices _customerServiceObj = new NavtechCustomerServices();
        
        //Service to add Orders
        [ResponseType(typeof(Order))]
        public HttpResponseMessage AddOrder(Order order)
        {
            try
            {
                if (!ModelState.IsValid) //if given values are not valid
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { OrderId = 0, Message = "Please enter valid Order details" });
                }
                else
                {
                    if (_customerServiceObj.IsCustomerExists(order.CustomerId))
                    {
                        db.Orders.Add(order);
                        db.SaveChanges();
                        db.OrderItems.AddRange(order.OrderItems);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new { OrderId = order.OrderId, Message = "Order added successfully" });
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, new { OrderId = 0, Message = "Provided customer details does not exists" });
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { OrderId = 0, Message = ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(long id)
        {
            return db.Orders.Count(e => e.OrderId == id) > 0;
        }
    }
}