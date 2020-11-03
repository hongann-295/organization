using DotNetNuke.Web.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modules.OrganizationOrganization.Components
{
    public class RouteConfig : IMvcRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapRoute("Organization", "default", "{controller}/{action}",
                new[] { " Modules.OrganizationOrganization.Controllers" });
        }
    }
}