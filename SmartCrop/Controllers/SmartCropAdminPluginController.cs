using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Security;
using Forte.SmartCrop.Models.ViewModels;

namespace Forte.SmartCrop.Business.Plugins
{
    [GuiPlugIn(DisplayName = "Manage SmartCrop", Area = PlugInArea.AdminMenu,
        Url = "/custom-plugins/smartcrop-plugin")]
    public class SmartCropAdminPluginController : Controller
    {
        // GET
        public ActionResult Index()
        {
            var images = GetAllImages();

            var model = new SmartCropAdminPluginViewModel {ImagesEnumerable = images};

            return View("~/modules/_protected/Forte.SmartCrop/Index.cshtml", model);
        }

        private static IEnumerable<ImageData> GetAllImages()
        {
            return Enumerable.Empty<ImageData>();
        }
    }
}