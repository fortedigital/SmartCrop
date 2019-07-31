using EPiServer.PlugIn;
using Forte.SmartFocalPoint.Models.ViewModels;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Forte.SmartFocalPoint.Models;

namespace Forte.SmartFocalPoint.Business
{
    [GuiPlugIn(DisplayName = "Manage SmartFocalPoint", Area = PlugInArea.AdminMenu,
        Url = "/custom-plugins/smartfocalpoint-plugin")]
    public class SmartFocalPointAdminPluginController : Controller
    {
        private readonly SmartFocalPointAdminPluginSettings _settings;
        private readonly IContentLoader _contentLoader;
        private readonly MediaFolder _mediaFolder;

        public SmartFocalPointAdminPluginController()
        {
            _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            _settings = new SmartFocalPointAdminPluginSettings();
            _mediaFolder = LoadMediaFolderStructure();
        }

        // GET
        public ActionResult Index()
        {

            var model = new SmartFocalPointAdminPluginViewModel {IsSmartFocalPointEnabled = GetSmartFocalPointSetting(), MediaFolder = _mediaFolder};

            return View("~/modules/_protected/Forte.SmartFocalPoint/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult ButtonAction(SmartFocalPointAdminPluginViewModel model)
        {
            _settings.SaveSettingsValue(model.IsSmartFocalPointEnabled);
            return View("~/modules/_protected/Forte.SmartFocalPoint/Index.cshtml", model);
        }

        public bool GetSmartFocalPointSetting()
        {
            return _settings.IsConnectionEnabled();
        }

        private MediaFolder LoadMediaFolderStructure()
        {
            var root = SiteDefinition.Current.GlobalAssetsRoot;
            var mediaFolder = new MediaFolder(root);

            MakeFolderStructure(mediaFolder);

            return mediaFolder;
        }

        private void MakeFolderStructure(MediaFolder parentFolder)
        {
            foreach (var folder in _contentLoader.GetChildren<ContentFolder>(parentFolder.FolderReference))
            {
                parentFolder.AddChildFolder(folder.ContentLink);
            }

            foreach (var childrenFolder in parentFolder.ChildrenFolders)
            {
                MakeFolderStructure(childrenFolder);
            }
        }

    }
}