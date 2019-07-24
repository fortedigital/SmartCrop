using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Logging;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using Forte.SmartFocalPoint.Models.Media;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using SiteDefinition = EPiServer.Web.SiteDefinition;

namespace Forte.SmartFocalPoint
{
    [ScheduledPlugIn(DisplayName = "Set FocalPoint For Unset Images", GUID = "DF91149F-796B-441F-A9C0-CF88D38FF58F",
        Description = "Goes over image files and updates FocalPoint property for unset images")]
    public class MediaUpdaterJob : ScheduledJobBase
    {
        private bool _stopSignaled;
        private readonly IContentRepository _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        private readonly ILogger _logger = LogManager.GetLogger();
        
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
            OnStatusChanged($"Starting execution of {this.GetType()}");

            var assetsRoot = SiteDefinition.Current.GlobalAssetsRoot;
            return UpdateImages(assetsRoot);
            
        }

        private string UpdateImages(ContentReference reference)
        {
            var imagesEnumerable = _contentRepository.GetDescendents(reference)
                .Where(r => _contentRepository.Get<IContent>(r) is ImageData)
                .Select(_contentRepository.Get<ImageData>);
            var images = imagesEnumerable as ImageData[] ?? imagesEnumerable.ToArray();

            var failedImagesStatuses = new List<string>();
            var imagesCount = images.Length;
            var updatedCount = 0;
            var skippedCount = 0;

            foreach (var image in images)
            {
                var returnedStatus = UpdateProperties(image);
                updatedCount++;
                if (returnedStatus == " ")
                    skippedCount++;

                if (!string.IsNullOrWhiteSpace(returnedStatus))
                {
                    failedImagesStatuses.Add(returnedStatus);
                }

                //For long running jobs periodically check if stop is signaled and if so stop execution
                if (_stopSignaled)
                {
                    return "Stop of job was called.\r\n" + GetStatusMessage(imagesCount, updatedCount, skippedCount, failedImagesStatuses);
                }
                
            }
            return "Image files' properties updated.\r\n" + GetStatusMessage(imagesCount, updatedCount, skippedCount, failedImagesStatuses);
        }

        private string UpdateProperties(ImageData image)
        {
            
            if(!(image is IFocalImageData focalImage))
                return $"{image.Name} is not of type {nameof(IFocalImageData)}";

            if (focalImage.FocalPoint != null)
                return " ";

            //republish image
            var file = _contentRepository.Get<ImageData>(image.ContentLink).CreateWritableClone() as ImageData;
            try
            {
                _contentRepository.Save(file, SaveAction.Publish | SaveAction.ForceCurrentVersion);
            }
            catch (AccessDeniedException ex)
            {
                _logger.Error(ex.Message);
                return $"{image.Name}: {ex.Message}";
            }

            return string.Empty;
        }

        private static string GetStatusMessage(int allImagesCount, int updatedImagesCount, int skippedImagesCount, List<string> returnStatuses)
        {
            var message = $"Processed images: {updatedImagesCount} out of {allImagesCount}, " +
                          $"{skippedImagesCount} skipped, {returnStatuses.Count} failed.\r\n";
            foreach (var statMsg in returnStatuses)
            {
                message = message + statMsg + "\r\n";
            }

            return message;
        }
    }

}
