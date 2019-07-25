using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Shell;
using Forte.SmartFocalPoint.Business;
using Forte.SmartFocalPoint.Models.Media;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;
using System.Drawing;

namespace Forte.SmartFocalPoint
{
    [ModuleDependency(typeof(ShellInitialization))]
    [InitializableModule]
    public class SmartFocalPointModule : IInitializableModule
    {
	    private const int MaxSize = 1024;

        public virtual CognitiveServicesConnector ServicesConnector { get; set; }
        public virtual ModuleUtilities Utilities { get; set; }
        public virtual SmartFocalPointAdminPluginSettings ConnectionSettings { get; set; }

        public void Initialize(InitializationEngine context)
        {
            context.Locate.ContentEvents().PublishingContent += HandlePublishingContent;
            ConnectionSettings = new SmartFocalPointAdminPluginSettings();
            ServicesConnector = new CognitiveServicesConnector();
            Utilities = new ModuleUtilities();
        }

        private void HandlePublishingContent(object sender, ContentEventArgs e)
        {
            if (!(e.Content is IFocalImageData imageFile))
                return;

            //if FP is set then leave it
            if (imageFile.FocalPoint != null)
                return;

            //if FP is null and last version is not then editor set it so, leave it
            if (!Utilities.IsLastVersionFocalPointNull(imageFile))
                return;

            if (!ConnectionSettings.IsConnectionEnabled())
                return;
            
            using (var stream = Utilities.GetBlobStream(imageFile))
            {
                var originalImage = Image.FromStream(stream);

                var resizedImage = Utilities.ResizeImage(originalImage, MaxSize);

                var boundingRect = ServicesConnector.GetAreaOfInterest(resizedImage);

                if (boundingRect == null)
                    return;

                var scaleX = 1.0 / (resizedImage.Width / (double) originalImage.Width);
                var scaleY = 1.0 / (resizedImage.Height / (double) originalImage.Height);

                var areaOfInterestX = (int) (boundingRect.X * scaleX);
                var areaOfInterestY = (int) (boundingRect.Y * scaleY);
                var areaOfInterestWidth = (int) (boundingRect.W * scaleX);
                var areaOfInterestHeight = (int) (boundingRect.H * scaleY);

                var middlePointX = areaOfInterestX + areaOfInterestWidth / 2;
                var middlePointY = areaOfInterestY + areaOfInterestHeight / 2;
                imageFile.FocalPoint = new FocalPoint()
                {
                    X = 100 * middlePointX / (double) originalImage.Width,
                    Y = 100 * middlePointY / (double) originalImage.Height
                };
            }

        }

        public void Uninitialize(InitializationEngine context)
        {
            context.Locate.ContentEvents().PublishingContent -= HandlePublishingContent;
        }

    }
}