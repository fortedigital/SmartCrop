using System;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Logging;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using Forte.SmartFocalPoint.Models.Media;
using SiteDefinition = EPiServer.Web.SiteDefinition;

namespace Forte.SmartFocalPoint
{
    
    public class MediaUpdaterJob : ScheduledJobBase
    {
        private bool _stopSignaled;
        private readonly IContentRepository _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private readonly ILogger Logger = LogManager.GetLogger();

        protected bool SetForAll;

        public MediaUpdaterJob()
        {
            IsStoppable = true;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            _stopSignaled = true;
        }

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged(String.Format("Starting execution of {0}", this.GetType()));

            var assetsRoot = SiteDefinition.Current.GlobalAssetsRoot;
            return UpdateChildren(assetsRoot);
            
        }

        private string UpdateChildren(ContentReference reference)
        {
            var childrenFolders = _contentRepository.GetChildren<ContentFolder>(reference);
            var images = _contentLoader.GetChildren<ImageData>(reference);
            foreach (var image in images)
            {
                UpdateProperties(image);
            }

            //For long running jobs periodically check if stop is signaled and if so stop execution
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            foreach (var folder in childrenFolders)
            {
                UpdateChildren(folder.ContentLink);
            }

            return "Image files' properties updated";
        }

        private void UpdateProperties(ImageData image)
        {
            
            if(!(image is FocalImageData focalImage))
                return;

            if (SetForAll || focalImage.FocalPoint == null)
            {
                //republish image
                var file = _contentRepository.Get<ImageData>(image.ContentLink).CreateWritableClone() as ImageData;
                try
                {
                    _contentRepository.Save(file, SaveAction.Publish | SaveAction.ForceCurrentVersion);
                }
                catch (AccessDeniedException ex)
                {
                    Logger.Error(ex.Message);
                }

            }

        }
    }

    [ScheduledPlugIn(DisplayName = "Set FocalPoint For Unset Images", GUID = "DF91149F-796B-441F-A9C0-CF88D38FF58F",
        Description = "Goes over image files and updates focal point properties for unset images")]
    public class UpdateUnsetImages : MediaUpdaterJob
    {

        public UpdateUnsetImages()
        {
            SetForAll = false;
        }

    }

    [ScheduledPlugIn(DisplayName = "Set FocalPoint For All Images", GUID = "515A9EFA-910F-4A19-8B50-DE60678A097E",
        Description = "Goes over image files and updates all of them with focal point properties")]
    public class UpdateAllImages : MediaUpdaterJob
    {

        public UpdateAllImages()
        {
            SetForAll = true;
        }
    }

}
