using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Forte.SmartFocalPoint.Business
{
    [InitializableModule]
    public class CustomRouteInitializer : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute("Default", "custom-plugins/smartfocalpoint-plugin/{action}",
                new
                {
                    controller = "SmartFocalPointAdminPlugin", action = "Index"
                });
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}