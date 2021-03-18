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
    public class ItemsController : ApiController
    {
        private NavtechEF db = new NavtechEF();

     
        //method to add items
        [ResponseType(typeof(Item))]
        public HttpResponseMessage AddItems(List<Item> items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Please enter valid Item details" });
                }
                else
                {
                    db.Items.AddRange(items);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Items added successfully" });
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { Message = ex.Message });
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
        private bool ItemExists(long id)
        {
            return db.Items.Count(e => e.ItemId == id) > 0;
        }
    }
}