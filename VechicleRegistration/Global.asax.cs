using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;
using System.Data.Entity;
using VechicleRegistration.Context;
using VechicleRegistration.Models;
using System.Configuration;

namespace VechicleRegistration
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer<VRContext>(new VRDataContextInitializer());
            //Database.SetInitializer<Models.VRContext>(new VRDataContextInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); 
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //. Configuration..Add(new AuthorizeAttribute());
        }
    }
}
