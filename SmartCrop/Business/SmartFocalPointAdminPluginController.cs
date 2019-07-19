using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Forte.SmartFocalPoint.Models.Media;
using Forte.SmartFocalPoint.Models.ViewModels;

namespace Forte.SmartFocalPoint.Business
{
    [GuiPlugIn(DisplayName = "Manage SmartFocalPoint", Area = PlugInArea.AdminMenu,
        Url = "/custom-plugins/smartfocalpoint-plugin")]
    public class SmartFocalPointAdminPluginController : Controller
    {
        private SmartFocalPointAdminPluginSettings _settings;

        public SmartFocalPointAdminPluginController()
        {
            _settings = new SmartFocalPointAdminPluginSettings();
        }

        // GET
        public ActionResult Index()
        {

            var model = new SmartFocalPointAdminPluginViewModel {IsSmartCropEnabled = GetSmartFocalPointSetting()};

            return View("~/modules/_protected/Forte.SmartFocalPoint/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult Action(SmartFocalPointAdminPluginViewModel model)
        {
            _settings.SaveSettings(model.IsSmartCropEnabled);
            return View("~/modules/_protected/Forte.SmartFocalPoint/Index.cshtml", model);
        }

        public bool GetSmartFocalPointSetting()
        {
            return _settings.LoadSettings();
        }
        
    }
}