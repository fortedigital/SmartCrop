using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Forte.SmartCrop.Models.Media;
using Forte.SmartCrop.Models.ViewModels;

namespace Forte.SmartCrop.Business
{
    [GuiPlugIn(DisplayName = "Manage SmartCrop", Area = PlugInArea.AdminMenu,
        Url = "/custom-plugins/smartcrop-plugin")]
    public class SmartCropAdminPluginController : Controller
    {
        private SmartCropAdminPluginSettings _settings;

        public SmartCropAdminPluginController()
        {
            _settings = new SmartCropAdminPluginSettings();
        }

        // GET
        public ActionResult Index()
        {

            var model = new SmartCropAdminPluginViewModel {IsSmartCropEnabled = GetSmartCropSetting()};

            return View("~/modules/_protected/Forte.SmartCrop/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult Action(SmartCropAdminPluginViewModel model)
        {
            _settings.SaveSettings(model.IsSmartCropEnabled);
            return View("~/modules/_protected/Forte.SmartCrop/Index.cshtml", model);
        }

        public bool GetSmartCropSetting()
        {
            return _settings.LoadSettings();
        }
        
    }
}