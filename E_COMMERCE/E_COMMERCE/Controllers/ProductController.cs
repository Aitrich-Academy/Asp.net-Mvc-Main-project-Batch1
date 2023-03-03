using DAL.Manager;
using DAL.Models;
using E_COMMERCE.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;

namespace E_COMMERCE.Controllers
{
    [RoutePrefix("Api/Product")]
    public class ProductController : ApiController
    {
        [Route("proInsert")]
        [HttpPost]
        public string proInsert(Ent_Product obj)
        {
            ProductManager mng = new ProductManager();
            Ent_Product objEnt = obj;
            PRODUCT pro = new PRODUCT();
            pro.PRO_NAME = objEnt.name;
            pro.PROCAT_ID = objEnt.categoryId;
            pro.PRO_DESC = objEnt.description;
            pro.PRO_STOCK = objEnt.stock;
            pro.PRO_PRICE = objEnt.price;
            pro.PRO_IMAGE = objEnt.image;
            pro.PRO_STATUS = objEnt.status;
            pro.PRO_CREATEBY = objEnt.createdBy;
            pro.PRO_CREATEDATE = objEnt.createdDate;
            pro.PRO_MODIBY = objEnt.lastmodifiedBy;
            pro.PRO_MODIDATE = objEnt.lastModifiedDate;
            return mng.proInsert(pro);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("allPro")]
        public List<Ent_Product> allPro(int categoryId = 0)
        {
            ProductManager mngr = new ProductManager();
            List<Ent_Product> return_List = new List<Ent_Product>();
            List<PRODUCT> tbl_obj = mngr.allPro(categoryId);
            if (tbl_obj.Count != 0)
            {

                foreach (var Ob in tbl_obj)
                {
                    return_List.Add(new Ent_Product
                    {
                        id = Ob.PRO_ID,
                        name = Ob.PRO_NAME,
                        categoryId = Ob.PROCAT_ID,
                        description = Ob.PRO_DESC,
                        stock = Ob.PRO_STOCK,
                        price = Ob.PRO_PRICE,
                        image = Ob.PRO_IMAGE,
                        status = Ob.PRO_STATUS,
                        createdBy = Ob.PRO_CREATEBY,
                        createdDate = Ob.PRO_CREATEDATE,
                        lastmodifiedBy = Ob.PRO_MODIBY,
                        lastModifiedDate = Ob.PRO_MODIDATE
                    });
                }
            }
            return return_List;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("productByID")]
        public Ent_Product productByID(string id)
        {
            ProductManager mngr = new ProductManager();
            Ent_Product return_Obj = new Ent_Product();
            PRODUCT tbl_obj = mngr.proDetails(Convert.ToInt32(id));

            if (tbl_obj != null)
            {
                return_Obj.id = tbl_obj.PRO_ID;
                return_Obj.name = tbl_obj.PRO_NAME;
                return_Obj.categoryId = tbl_obj.PROCAT_ID;
                return_Obj.categoryname = tbl_obj.CATEGORY.CAT_NAME;
                return_Obj.description = tbl_obj.PRO_DESC;
                return_Obj.stock = tbl_obj.PRO_STOCK;
                return_Obj.price = tbl_obj.PRO_PRICE;
                return_Obj.image = tbl_obj.PRO_IMAGE;
                return_Obj.status = tbl_obj.PRO_STATUS;
                return_Obj.createdBy = tbl_obj.PRO_CREATEBY;
                return_Obj.createdDate = tbl_obj.PRO_CREATEDATE;
                return_Obj.lastmodifiedBy = tbl_obj.PRO_MODIBY;
                return_Obj.lastModifiedDate = tbl_obj.PRO_MODIDATE;
            }
            return return_Obj;
        }
        public HttpResponseMessage Delete(int id)
        {
            ProductManager mngr = new ProductManager();
            mngr.proDelete(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Category Deleted Successfully");
        }
        public HttpResponseMessage Put(int id, Ent_Product obj)
        {
            Ent_Product ent = obj;
            ProductManager mngr = new ProductManager();
            PRODUCT pro = new PRODUCT();
            pro.PRO_ID = id;
            pro.PRO_NAME = ent.name;
            pro.PROCAT_ID = ent.categoryId;
            pro.PRO_DESC = ent.description;
            pro.PRO_STOCK = ent.stock;
            pro.PRO_PRICE = ent.price;
            pro.PRO_IMAGE = ent.image;
            pro.PRO_CREATEBY = ent.createdBy;
            pro.PRO_CREATEDATE = ent.createdDate;
            pro.PRO_MODIBY = ent.lastmodifiedBy;
            pro.PRO_MODIDATE = ent.lastModifiedDate;
            mngr.proUpdate(pro);
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("SearchPro")]
        [HttpPost]
        public  List<Ent_Product> SearchPro(String proName)
        {
           
            ProductManager mngr = new ProductManager();
            PRODUCT objPro = new PRODUCT();
            List<Ent_Product> list = new List<Ent_Product>();
            List<PRODUCT> obj = mngr.SearchPro(proName);
            foreach(var srch in obj)
            {
                list.Add(new Ent_Product
                {
                  name=srch.PRO_NAME,
                  categoryname=srch.CATEGORY.CAT_NAME,
                  description=srch.PRO_DESC,
                  image=srch.PRO_IMAGE,
                  stock=srch.PRO_STOCK,
                  price = srch.PRO_PRICE
                });
            }
            return list;
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
