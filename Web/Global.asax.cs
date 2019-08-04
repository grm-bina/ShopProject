using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var temp= DelatedSessionData.GetInstance;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            List<int> currentUserCart = (List<int>)Session["productsCurrentUser"];
            if(currentUserCart!=null && currentUserCart.Count > 0)
            {
                foreach (var item in currentUserCart)
                    DelatedSessionData.ShoppingCart.Enqueue(item);
            }
            Session["productsCurrentUser"] = null;
        }
    }
}
