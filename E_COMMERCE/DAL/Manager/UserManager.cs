using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manager
{
    public class UserManager
    {
        E_COMMERCEEntities dbhelper = new E_COMMERCEEntities();
        public string AddUser(USER usr)
        {
            int result = 0;

            dbhelper.USERs.Add(usr);
            result = dbhelper.SaveChanges();

            if (result > 0)
            {
                return usr.USER_ID.ToString();
            }
            else
            {
                return "Error ";
            }
        }
        public List<USER> UserDetails(int id)
        {
            return dbhelper.USERs.Where(p => p.USER_ID == id).ToList();
        }
        public string UpdateUser(USER ur)
        {
            int result = 0;
            var id = dbhelper.USERs.SingleOrDefault(p => p.USER_ID == ur.USER_ID);
            if (id == null)
                return "Error";
            else
            {
                id.USER_NAME = ur.USER_NAME;
                id.USER_EMAIL = ur.USER_EMAIL;
                id.USER_PHONE = ur.USER_PHONE;
                id.USER_ADDRESS = ur.USER_ADDRESS;
                id.USER_MODIDATE = ur.USER_MODIDATE;

                dbhelper.Entry(id).State = System.Data.Entity.EntityState.Modified;
                dbhelper.SaveChanges();
                return id.USER_ID.ToString();
            }
        }

        public void UserDelete(int id)
        {
            var id1 = dbhelper.USERs.SingleOrDefault(p => p.USER_ID == id);

            dbhelper.USERs.Remove(id1);
            dbhelper.SaveChanges();
        }

        public USER UserLogin(USER ur)
        {
            var log = dbhelper.USERs.Where(x => x.USER_EMAIL.Equals(ur.USER_EMAIL) &&
                                  x.USER_PASSWORD.Equals(ur.USER_PASSWORD)).FirstOrDefault();

            return log;
        }


        //public string SelectMailid(int id)
        //{
        //    var userid= from uid in dbhelper.ORDERS
        //                where uid.ORD_ID == id
        //                select uid;


        //    int usid = userid.FirstOrDefault().ORD_USERID;

        //    var results = from user in dbhelper.USERs
        //                  where user.USER_ID ==usid
        //                  select user;

        //    string mailid = results.FirstOrDefault().USER_EMAIL;
        //    return mailid;
        //}




        public string userPatch(USER ur)
        {

            var objUser = dbhelper.USERs.Where(e => e.USER_ID == ur.USER_ID && e.USER_STATUS != "D").SingleOrDefault();
            if (objUser == null)
            {
                return "error";
            }
            else
            {
                objUser.USER_NAME = ur.USER_NAME == null ? objUser.USER_NAME : ur.USER_NAME;
                objUser.USER_EMAIL = ur.USER_EMAIL == null ? objUser.USER_EMAIL : ur.USER_EMAIL;
                objUser.USER_PHONE = ur.USER_PHONE == null ? objUser.USER_PHONE : ur.USER_PHONE;
                objUser.USER_ADDRESS = ur.USER_ADDRESS == null ? objUser.USER_ADDRESS : ur.USER_ADDRESS;
                objUser.USER_IMAGE = ur.USER_IMAGE;
                objUser.USER_ROLE = ur.USER_ROLE == null ? objUser.USER_ROLE : ur.USER_ROLE; ;
                objUser.USER_PASSWORD = ur.USER_PASSWORD == null ? objUser.USER_PASSWORD : ur.USER_PASSWORD;
                objUser.USER_STATUS = ur.USER_STATUS == null ? objUser.USER_STATUS : ur.USER_STATUS;

                objUser.USER_MODIDATE = DateTime.Now.ToString();


                dbhelper.Entry(objUser).State = EntityState.Modified;
                dbhelper.SaveChanges();
                return objUser.USER_ID.ToString();
            }
        }

    }

}
