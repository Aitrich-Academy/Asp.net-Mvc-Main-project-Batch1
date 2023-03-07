using DAL.Manager;
using DAL.Models;
using ECOMMERSE.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Razor.Tokenizer;

namespace ECOMMERSE.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        OrderManager mgr = new OrderManager();
        UserManager userManager = new UserManager();
        MailManager mailManager = new MailManager();
        Ent_Mails mil = new Ent_Mails();
        public HttpResponseMessage Post(Ent_Order ent)
        {
            ORDER ord = new ORDER();
            ord.ORD_USERID = ent.userid;
            ord.ORD_PROID = ent.productid;
            ord.ORD_QTY = ent.quantity;


            int tot = mgr.GetPrice(ord);
            ord.ORD_TOTAL = ent.quantity * tot;


            ord.ORD_STATUS = "A";
            ord.ORD_CREATEBY = "Test";
            ord.ORD_CREATEDATE = DateTime.Now.ToString();
            ord.ORD_MODIBY = "Test1";
            ord.ORD_MODIDATE = DateTime.Now.ToString();

            string result = mgr.AddOrder(ord);

            if (result == "Error")
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
            else
            {

                return Request.CreateResponse(HttpStatusCode.OK, "Added Successfully");

            }
        }

        [Route("OrderedItems")]
        [HttpPost]
        public HttpResponseMessage OrderFullItems(Ent_Order ent)
        {

            try
            {

                ORDER order = new ORDER();
                order.ORD_USERID = ent.userid;

                string mailid = mgr.SelectMailid(order);
                mil.Email = mailid;


                List<ORDER> ord1 = mgr.OrderFullData(order);
                foreach (var obj in ord1)
                {
                    int tot = mgr.GetPriceByProduct(obj.ORD_PROID);
                    order.ORD_TOTAL = obj.ORD_QTY * tot;
                    mil.Body += "<h3>Quantity    :   " + obj.ORD_QTY + "\nPrice    :    " + tot + "\nTotal   :   " + obj.ORD_TOTAL + "\n</h3>";
                }

                mil.Subject = "Order Details";

                string result = mailManager.SendEmailToAdmin(mil);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        [Route("SendMailtoUser")]
        [HttpPost]
        public HttpResponseMessage SendMailtoUser(Ent_Order ent)
        {
            try
            {

                ORDER order = new ORDER();
                order.ORD_USERID = ent.userid;

                string mailid = mgr.SelectMailid(order);
                mil.Email = mailid;


                mil.Body = "OrderAccepted";
                mil.Subject = "Order Details";


                List<ORDER> ord1 = mgr.OrderFullData(order);

                foreach (var obj in ord1)
                {
                    obj.ORD_USERID = obj.ORD_USERID;
                    obj.ORD_STATUS = "D";
                    mgr.UpdateStatus(obj);
                }

                string result = mailManager.SendMailToUser(mil);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }




        public HttpResponseMessage Delete(int id)
        {
            ORDER ord = new ORDER();
            ord.ORD_ID = id;
            mgr.OrderCancelation(ord);

            return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
        }

        [Route("SearchByDate")]
        [HttpGet]
        public List<Ent_Order> GetByDate(Ent_Order or)
        {
            List<Ent_Order> list = new List<Ent_Order>();
            ORDER obj1 = new ORDER();

            obj1.ORD_MODIDATE = or.modifydate;

            List<ORDER> ord = mgr.OrderByDate(obj1);
            foreach (var obj in ord)
            {
                list.Add(new Ent_Order
                {
                    userid = obj.ORD_USERID,
                    quantity = obj.ORD_QTY,
                    total = obj.ORD_TOTAL,
                });
            }
            return list;
        }

        [Route("SearchByProduct")]
        [HttpGet]
        public List<Ent_Order> SearchByProduct(int proid)
        {
            List<Ent_Order> list = new List<Ent_Order>();
            List<ORDER> ord = mgr.Product_OrderDetails(proid);

            foreach (var obj in ord)
            {
                list.Add(new Ent_Order
                {
                    userid = obj.ORD_USERID,
                    quantity = obj.ORD_QTY,
                    total = obj.ORD_TOTAL,
                });
            }
            return list;
        }


        [Route("GetByUser")]
        [HttpGet]
        public List<Ent_Order> GetByUser(Ent_Order or)
        {
            List<Ent_Order> list = new List<Ent_Order>();
            ORDER obj1 = new ORDER();
            obj1.ORD_USERID = or.userid;
            List<ORDER> ord = mgr.Ordered_UserDetails(obj1);

            foreach (var obj in ord)
            {
                list.Add(new Ent_Order
                {
                    userid = obj.ORD_USERID,
                    quantity = obj.ORD_QTY,
                    total = obj.ORD_TOTAL,
                });
            }
            return list;
        }
    }
}
