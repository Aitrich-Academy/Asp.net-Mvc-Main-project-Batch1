using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manager
{
    public class ProductManager
    {
        E_COMMERCEEntities dB_obj = new E_COMMERCEEntities();
        public string proInsert(PRODUCT Obj)
        {
            int result = 0;
            var objpro = dB_obj.PRODUCTS.Where(e => e.PRO_NAME == Obj.PRO_NAME && e.PRO_STATUS != "D").SingleOrDefault();
            if (objpro == null)
            {
                Obj.PRO_CREATEDATE = DateTime.Now.ToString();
                Obj.PRO_MODIDATE = DateTime.Now.ToString();
                Obj.PRO_STATUS = "A";
                Obj.PRO_CREATEBY = "MVC";
                Obj.PRO_MODIBY = "MVC";
                dB_obj.PRODUCTS.Add(Obj);
                result = dB_obj.SaveChanges();
            }

            if (result > 0)
            {
                return Obj.PRO_ID.ToString();
            }
            else
            {
                return "Error";
            }

        }
        public List<PRODUCT> allPro(int catId)
        {
            if (catId != 0)
            {
                return dB_obj.PRODUCTS.Where(e =>e.PROCAT_ID == catId && e.PRO_STATUS != "D").ToList();
            }
            return dB_obj.PRODUCTS.Where(e=>e.PRO_STATUS!="D").ToList();
        }
        public PRODUCT proDetails(int Id)
        {
            PRODUCT return_Obj = new PRODUCT();
            return return_Obj = dB_obj.PRODUCTS.Where(e => e.PRO_ID == Id && e.PRO_STATUS != "D").SingleOrDefault();
        }
        public void proDelete(int id)
        {
            var proId = dB_obj.PRODUCTS.Where(e => e.PRO_ID == id).SingleOrDefault();
            dB_obj.PRODUCTS.Remove(proId);
            dB_obj.SaveChanges();
        }
        public void proUpdate(PRODUCT pro)
        {
            var objpro = dB_obj.PRODUCTS.Where(e => e.PRO_ID == pro.PRO_ID && e.PRO_STATUS != "D").SingleOrDefault();
            if (objpro != null)
            {
                objpro.PRO_NAME = pro.PRO_NAME;
                objpro.PROCAT_ID = pro.PROCAT_ID;
                objpro.PRO_DESC = pro.PRO_DESC;
                objpro.PRO_STOCK = pro.PRO_STOCK;
                objpro.PRO_PRICE = pro.PRO_PRICE;
                objpro.PRO_IMAGE = pro.PRO_IMAGE;
                objpro.PRO_STATUS = "A";
                objpro.PRO_CREATEBY = "MVC";
                objpro.PRO_CREATEDATE = DateTime.Now.ToString();
                objpro.PRO_MODIBY = "MVC";
                objpro.PRO_MODIDATE = DateTime.Now.ToString();
                dB_obj.Entry(objpro).State = EntityState.Modified;
                dB_obj.SaveChanges();
            }
        }
        public List<PRODUCT> SearchPro(string proName)
        {
            try
            {
                return dB_obj.PRODUCTS.Where(e => e.PRO_NAME.Contains(proName) && e.PRO_STATUS != "D").ToList();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
