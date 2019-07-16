using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Forte.SmartCrop.Business.Plugins
{
    [InitializableModule]
    public class CustomRouteInitializer : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute("Default", "custom-plugins/smartcrop-plugin/{action}",
                new
                {
                    controller = "SmartCropAdminPlugin", action = "Index"
                });
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}