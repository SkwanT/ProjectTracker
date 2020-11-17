using ProjectTracker.DAL;
using ProjectTracker.Infrastructure;
using ProjectTracker.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace ProjectTracker
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
            GlobalFilters.Filters.Add(new UserAuditFilter());

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }


        protected void Application_Error(Object sender, EventArgs e)
        {
            var raiseException = Server.GetLastError();
            ErrorLogging(raiseException.Message);

        }


        public static void ErrorLogging(string Message)
        {
            string username = "";
            try
            {
                FormsIdentity myid = (FormsIdentity)HttpContext.Current.User.Identity;
                string[] userdata = myid.Ticket.UserData.ToString().Split(';');
                username = userdata[0];
            }
            catch
            {
            }


            string strPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\log_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + ".txt";

            //string strPath = @"c:\temp\log_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + ".txt";
            if (!File.Exists(strPath))
            {
                File.Create(strPath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(strPath))
            {
                sw.WriteLine("--------------------------------------------------------------------------");
                sw.WriteLine("DateTime: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                sw.WriteLine("Message:  " + Message);
                sw.WriteLine("UserIP:   " + HttpContext.Current.Request.UserHostAddress);
                sw.WriteLine("UserID:   " + username);
            }

        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    try
                    {
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        if (authTicket != null && !authTicket.Expired)
                        {
                            var roles = authTicket.UserData.Split(';');
                            var r = roles[1].Split(',');
                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), r);
                        }
                    }
                    catch (Exception ex)
                    {

                        ProjectTracker.MvcApplication.ErrorLogging("Application_PostAuthenticateRequest: " + ex.Message);

                    }

                }
            }
        }
    }




    public class UserAuditFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AuditTB objaudit = new AuditTB();

            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var request = filterContext.HttpContext.Request;

            if (request.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)filterContext.HttpContext.User.Identity;
                string[] userdata = id.Ticket.UserData.ToString().Split(';');
                int UserID = Convert.ToInt32(userdata[0]);
                objaudit.UserID = UserID;
                objaudit.LoginStatus = "A";
            }
            else
            {
                objaudit.UserID = null;
                objaudit.LoginStatus = "N";
            }

            objaudit.ID = 0;
            objaudit.SessionID = HttpContext.Current.Session.SessionID;
            objaudit.IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(objaudit.IPAddress))
            {
                objaudit.IPAddress = request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(objaudit.IPAddress))
            {
                objaudit.IPAddress = request.UserHostAddress;
            }
            objaudit.PageAccessed = request.RawUrl;
            objaudit.LoggedInAt = DateTime.Now;
            if (actionName == "LogOff")
            {
                objaudit.LoggedOutAt = DateTime.Now;
            }
            objaudit.ControllerName = controllerName;
            objaudit.ActionName = actionName;

            AuditContext auditContext = new AuditContext();
            auditContext.AuditTBs.Add(objaudit);
            auditContext.SaveChanges();

            base.OnActionExecuting(filterContext);
        }

    }


}
