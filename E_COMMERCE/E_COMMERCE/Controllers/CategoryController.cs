using DAL.Manager;
using DAL.Models;
using E_COMMERCE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace E_COMMERCE.Controllers
{
    [RoutePrefix("Api/Category")]
    public class CategoryController : ApiController
    {

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("catRegistration")]
        [HttpPost]
        public string catRegistration(Ent_Category obj)
        {
            CategoryManager mng = new CategoryManager();
            Ent_Category objEnt = obj;
            CATEGORY Reg = new CATEGORY();
            Reg.CAT_NAME = objEnt.name;
            Reg.CAT_DESC = objEnt.description;
            Reg.CAT_IMAGE = objEnt.image;
            Reg.CAT_STATUS = objEnt.status;
            Reg.CAT_CREATEDBY = objEnt.createdBy;
            Reg.CAT_CREATEDDATE = objEnt.createdDate;
            Reg.CAT_MODIBY = objEnt.lastmodifiedBy;
            Reg.CAT_MODIDATE = objEnt.lastModifiedDate;          
            return mng.catRegistration(Reg);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("allCat")]
        public List<Ent_Category> allCat()
        {
            CategoryManager mngr = new CategoryManager();
            List<Ent_Category> return_List = new List<Ent_Category>();
            List<CATEGORY> tbl_obj = mngr.allcat();
            if (tbl_obj.Count != 0)
            {

                foreach (var Ob in tbl_obj)
                {
                    return_List.Add(new Ent_Category
                    {
                        id=Ob.CAT_ID,
                        name=Ob.CAT_NAME,
                        description=Ob.CAT_DESC,
                        image=Ob.CAT_IMAGE,
                        status=Ob.CAT_STATUS,
                        createdBy=Ob.CAT_CREATEDBY,
                        createdDate=Ob.CAT_CREATEDDATE,
                        lastmodifiedBy=Ob.CAT_MODIBY,
                        lastModifiedDate=Ob.CAT_MODIDATE
                    });
                }
            }
            return return_List;
        }
        public HttpResponseMessage Delete(int id)
        {
            CategoryManager mngr = new CategoryManager();
            mngr.catDelete(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Category Deleted Successfully");
        }
        public HttpResponseMessage Put(int id,Ent_Category obj)
        {
            Ent_Category ent = obj;
            CategoryManager mngr = new CategoryManager();
            CATEGORY cat = new CATEGORY();
            cat.CAT_ID = id;
            cat.CAT_NAME = ent.name;
            cat.CAT_DESC = ent.description;
            cat.CAT_IMAGE = ent.image;
            cat.CAT_STATUS = ent.status;
            cat.CAT_CREATEDBY = ent.createdBy;
            cat.CAT_CREATEDDATE = ent.createdDate;
            cat.CAT_MODIBY = ent.lastmodifiedBy;
            cat.CAT_MODIDATE = ent.lastModifiedDate;
            mngr.catUpdate(cat);
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("uploadFile")]
        public string UploadFile()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ?
                HttpContext.Current.Request.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(
                    HttpContext.Current.Server.MapPath("~/uploads"),
                    fileName
                );

                file.SaveAs(path);
            }

            return file != null ? "/uploads/" + file.FileName : null;
        }
    }
}
