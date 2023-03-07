using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manager
{
    public class OrderManager
    {
        E_COMMERCEEntities dbhelper = new E_COMMERCEEntities();
        public string AddOrder(ORDER ord)
        {
            int result = 0;

            dbhelper.ORDERS.Add(ord);
            result = dbhelper.SaveChanges();

            if (result > 0)
                return ord.ORD_ID.ToString();
            else
                return "Error ";
        }

        public List<ORDER> Product_OrderDetails(int id)
        {
            return dbhelper.ORDERS.Where(p => p.ORD_PROID == id).ToList();
        }

        public List<ORDER> Ordered_UserDetails(ORDER uid)
        {
            return dbhelper.ORDERS.Where(p => p.ORD_USERID == uid.ORD_USERID && p.ORD_STATUS != "D").ToList();
        }

        public void OrderCancelation(ORDER obj)
        {
            var id = dbhelper.ORDERS.SingleOrDefault(e => e.ORD_ID == obj.ORD_ID);

            dbhelper.ORDERS.Remove(id);
            dbhelper.SaveChanges();
        }

        public string SelectMailid(ORDER order)
        {
            var mailid = dbhelper.USERs.FirstOrDefault(e => e.USER_ID == order.ORD_USERID);

            return mailid.USER_EMAIL;
        }

        public int GetPrice(ORDER rd)
        {
            var tot = dbhelper.PRODUCTS.FirstOrDefault(e => e.PRO_ID == rd.ORD_PROID);
            return tot.PRO_PRICE;
        }

        public int GetPriceByProduct(int proid)
        {
            var tot = dbhelper.PRODUCTS.FirstOrDefault(e => e.PRO_ID == proid);
            return tot.PRO_PRICE;
        }

        public List<ORDER> OrderByDate(ORDER obj)
        {
            return dbhelper.ORDERS.Where(p => p.ORD_MODIDATE == obj.ORD_MODIDATE).ToList();
        }

        public List<ORDER> OrderFullData(ORDER obj)
        {
            return dbhelper.ORDERS.Where(p => p.ORD_USERID == obj.ORD_USERID).ToList();
        }
        public string UpdateStatus(ORDER obj)
        {
            var id = dbhelper.ORDERS.FirstOrDefault(p => p.ORD_USERID == obj.ORD_USERID);
            if (id == null)
                return "Error";
            else
            {
                id.ORD_STATUS = obj.ORD_STATUS;

                dbhelper.Entry(id).State = System.Data.Entity.EntityState.Modified;
                dbhelper.SaveChanges();
                return id.ORD_ID.ToString();
            }
        }

    }
}
