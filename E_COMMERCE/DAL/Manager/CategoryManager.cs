using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manager
{
    public class CategoryManager
    {
        E_COMMERCEEntities dB_obj = new E_COMMERCEEntities();
        public string catRegistration(CATEGORY Obj)
        {
            int result = 0;
            var objcat = dB_obj.CATEGORies.Where(e => e.CAT_NAME == Obj.CAT_NAME && e.CAT_STATUS != "D").SingleOrDefault();
            if (objcat == null)
            {
                Obj.CAT_STATUS = "A";
                Obj.CAT_CREATEDBY = "MVC";
                Obj.CAT_MODIBY = "MVC";
                dB_obj.CATEGORies.Add(Obj);
                result = dB_obj.SaveChanges();
            }         

            if (result > 0)
            {
                return Obj.CAT_ID.ToString();
            }
            else
            {
                return "Error";
            }

        }
        public List<CATEGORY> allcat()
        {
            return dB_obj.CATEGORies.Where(e => e.CAT_STATUS != "D").ToList();
        }
        public void catDelete(int id)
        {
            var ctId = dB_obj.CATEGORies.Where(e => e.CAT_ID == id).SingleOrDefault();
            dB_obj.CATEGORies.Remove(ctId);
            dB_obj.SaveChanges();
        }
        public void catUpdate(CATEGORY cat)
        {          
            var objcat = dB_obj.CATEGORies.Where(e => e.CAT_ID == cat.CAT_ID && e.CAT_STATUS != "D").SingleOrDefault();
            if (objcat != null)
            {
                objcat.CAT_NAME = cat.CAT_NAME;
                objcat.CAT_DESC = cat.CAT_DESC;
                objcat.CAT_IMAGE = cat.CAT_IMAGE;
                objcat.CAT_STATUS = cat.CAT_STATUS;
                objcat.CAT_CREATEDBY = "MVC";
                objcat.CAT_CREATEDDATE = DateTime.Now.ToString();
                objcat.CAT_MODIBY = "MVC";
                objcat.CAT_MODIDATE = DateTime.Now.ToString();
                objcat.CAT_STATUS = "A";
                dB_obj.Entry(objcat).State = EntityState.Modified;
                dB_obj.SaveChanges();
            }
        }
    }
}
