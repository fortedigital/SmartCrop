using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.PlugIn;
using Forte.SmartCrop.Models.ViewModels;

namespace Forte.SmartCrop.Business
{
    [GuiPlugIn(DisplayName = "Manage SmartCrop", Area = PlugInArea.AdminMenu,
        Url = "/custom-plugins/smartcrop-plugin")]
    public class SmartCropAdminPluginController : Controller
    {
        // GET
        public ActionResult Index()
        {

            var model = new SmartCropAdminPluginViewModel {IsSmartCropEnabled = true};

            return View("~/modules/_protected/Forte.SmartCrop/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult Action(SmartCropAdminPluginViewModel model)
        {
            //do action

            return View("~/modules/_protected/Forte.SmartCrop/Index.cshtml", model);
        }

    }
}