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
        // GET
        public ActionResult Index()
        {

            var model = new SmartCropAdminPluginViewModel {IsSmartCropEnabled = true};

            return View("~/modules/_protected/Forte.SmartCrop/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult Action(SmartCropAdminPluginViewModel model)
        {
            SetSmartCropProperty(model.IsSmartCropEnabled);
            return View("~/modules/_protected/Forte.SmartCrop/Index.cshtml", model);
        }

        private void SetSmartCropProperty(bool isEnabled)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var media = contentRepository.GetDescendents(SiteDefinition.Current.GlobalAssetsRoot)
                .Where(r => contentRepository.Get<IContent>(r) is FocalImageData).Select(contentRepository.Get<FocalImageData>);

            foreach (var image in media)
            {
                var file = contentRepository.Get<FocalImageData>(image.ContentLink).CreateWritableClone() as FocalImageData;
                file.SmartCropEnabled = isEnabled;
                contentRepository.Save(file, SaveAction.ForceCurrentVersion);
            }
        }

    }
}