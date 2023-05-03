using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AslLabApi.Controllers.ASL
{
    public class SessionController : Controller
    {
        //
        // GET: /Session/
        public void Index()
        {
            Session["loggedCompID"] = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
            Session["loggedUserID"] = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            Session["LoggedUserType"] = Convert.ToString(System.Web.HttpContext.Current.Session["LoggedUserType"]);
            Session["LoggedUserName"] = Convert.ToString(System.Web.HttpContext.Current.Session["LoggedUserName"]);
            Session["LoggedUserStatus"] = Convert.ToString(System.Web.HttpContext.Current.Session["LoggedUserStatus"]);

           
        }
	}
}