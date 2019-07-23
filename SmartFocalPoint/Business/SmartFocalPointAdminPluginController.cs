using EPiServer.PlugIn;
using Forte.SmartFocalPoint.Models.ViewModels;
using System.Web.Mvc;

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

            var model = new SmartFocalPointAdminPluginViewModel {IsSmartFocalPointEnabled = GetSmartFocalPointSetting()};

            return View("~/modules/_protected/Forte.SmartFocalPoint/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult Action(SmartFocalPointAdminPluginViewModel model)
        {
            _settings.SaveSettings(model.IsSmartFocalPointEnabled);
            return View("~/modules/_protected/Forte.SmartFocalPoint/Index.cshtml", model);
        }

        public bool GetSmartFocalPointSetting()
        {
            return _settings.IsConnectionEnabled();
        }
        
    }
}