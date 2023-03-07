using ECOMMERSE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Manager;
using System.Web.Configuration;
using System.Web.Http;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using System.Net.Http;
using System.Net;
using System.IO;
using ECOMMERSE.Utils;
using System.Net.Http.Headers;

namespace ECOMMERSE.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        UserManager mgr = new UserManager();

        [System.Web.Http.HttpGet]
        [Route("UserRegister")]
        [HttpPost]
        public string UserRegister(Ent_User user)
        {
            Ent_User ent = user;
            USER usr = new USER();
            usr.USER_NAME = ent.Name;
            usr.USER_EMAIL = ent.email;
            usr.USER_PASSWORD = ent.password;
            usr.USER_PHONE = ent.phone;
            usr.USER_ADDRESS = ent.address;
            usr.USER_IMAGE = ent.image;
            usr.USER_ROLE = "USER";
            usr.USER_STATUS = ent.status;
            usr.USER_CREATEBY = ent.Name;
            usr.USER_CREATEDATE = DateTime.Now.ToString();
            usr.USER_MODIBY = ent.Name;
            usr.USER_MODIDATE = DateTime.Now.ToString();

            // UserManager mgr = new UserManager();
            return mgr.AddUser(usr);

        }
        public HttpResponseMessage Put(int id, [FromBody] Ent_User user)
        {
            Ent_User ent = user;
            USER usr = new USER();

            usr.USER_ID = id;
            usr.USER_NAME = ent.Name;
            usr.USER_EMAIL = ent.email;
            usr.USER_PHONE = ent.phone;
            usr.USER_ADDRESS = ent.address;
            usr.USER_MODIDATE = DateTime.Now.ToString();

            // UserManager mgr = new UserManager();
            string result = mgr.UpdateUser(usr);
            try
            {
                if (result == "Error")
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not found ");
                else
                    return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Delete(int id)
        {
            // UserManager mgr = new UserManager();
            mgr.UserDelete(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully");
        }

        public HttpResponseMessage Get(int id)
        {
            //  UserManager mgr = new UserManager();
            List<Ent_User> ent = new List<Ent_User>();
            try
            {
                List<USER> list = mgr.UserDetails(id);
                if (list.Count != 0)
                {
                    foreach (var obj in list)
                    {
                        ent.Add(new Ent_User
                        {
                            Name = obj.USER_NAME,
                            email = obj.USER_EMAIL,
                            address = obj.USER_ADDRESS,
                            phone = obj.USER_PHONE,
                            createdate = obj.USER_CREATEDATE,
                        });
                    }
                }
                return Request.CreateResponse(HttpStatusCode.Found, ent);
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data found");
            }
        }


        //[System.Web.Http.AcceptVerbs("GET", "POST")]
        //[System.Web.Http.HttpGet]
        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage Login(Ent_User user)
        {
            Ent_User ent = user;
            USER usr = new USER();

            usr.USER_EMAIL = ent.email;
            usr.USER_PASSWORD = ent.password;

            USER result = mgr.UserLogin(usr);

            if (result != null)
            {
                String token = TokenManager.GenerateToken(result);
                LoginResponseDTO loginResponseDTO = new LoginResponseDTO();
                loginResponseDTO.Token = token;
                loginResponseDTO.email = result.USER_EMAIL;
                loginResponseDTO.user_id = result.USER_ID;
                loginResponseDTO.address = result.USER_ADDRESS;
                loginResponseDTO.phone = result.USER_PHONE;
                loginResponseDTO.role = result.USER_ROLE;
                loginResponseDTO.name = result.USER_NAME;

                ResponseDataDTO response = new ResponseDataDTO(true, "Success", loginResponseDTO);
                return Request.CreateResponse(HttpStatusCode.OK, response);
                //return Request.CreateErrorResponse(HttpStatusCode.OK, result);

                return Request.CreateErrorResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid User name and password !");
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("uploadFile")]
        public HttpResponseMessage UploadFile()
        {
            AuthenticationHeaderValue authorization = Request.Headers.Authorization;
            if (authorization == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new
                {
                    message = "Please Login"
                });
            }
            Ent_User usersDTO = TokenManager.ValidateToken(authorization.Parameter);
            if (usersDTO != null && usersDTO.Id != null && usersDTO.role == "User")
            {
                var file = HttpContext.Current.Request.Files.Count > 0 ?
              HttpContext.Current.Request.Files[0] : null;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    byte[] byteArr;
                    using (var memoryStream = new MemoryStream())
                    {
                        file.InputStream.CopyTo(memoryStream);
                        byteArr = memoryStream.ToArray();
                    }

                    USER ur = new USER();
                    //id needs to take from token
                    ur.USER_ID = 2;
                    ur.USER_IMAGE = byteArr;
                    var res = mgr.userPatch(ur);
                    return Request.CreateResponse(HttpStatusCode.OK, res);

                }
                return null;

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new
                {
                    message = "Please Login"
                });
            }


        }

    }
}