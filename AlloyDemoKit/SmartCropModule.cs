using System;
using System.IO;
using AlloyDemoKit.Models.Media;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Shell;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace AlloyDemoKit
{
    [ModuleDependency(typeof(ShellInitialization))]
    [InitializableModule]
    public class SmartCropModule : IInitializableModule
    {
	    const string ApiKey = "0aae81ed8f2f43fbb05547107a53decd";

		public void Initialize(InitializationEngine context)
        {
            context.Locate.ContentEvents().PublishingContent += new EventHandler<ContentEventArgs>(this.HandlePublishingContent);
        }

        private void HandlePublishingContent(object sender, ContentEventArgs e)
        {
            if (e.Content is ImageFile)
            {
                var imageFile = (ImageFile)e.Content;

                if (imageFile.SmartCropEnabled)
                {
                    using (var stream = ReadBlob(imageFile))
                    {
	                    var rect = GetAreaOfInterest(stream);

	                    imageFile.AreaOfInterestX = rect.X;
	                    imageFile.AreaOfInterestY = rect.Y;
	                    imageFile.AreaOfInterestWidth = rect.W;
	                    imageFile.AreaOfInterestHeight = rect.H;
					}
                }
            }

        }

        private static MemoryStream ReadBlob(ImageFile content)
        {
            using (var stream = content.BinaryData.OpenRead())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                var memoryStream = new MemoryStream(buffer, writable: false);
                return memoryStream;
            }
        }

        private static BoundingRect GetAreaOfInterest(Stream imageStream)
        {
	        var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(ApiKey))
	        {
		        Endpoint = "https://westcentralus.api.cognitive.microsoft.com"
	        };

	        var result = client.GetAreaOfInterestInStreamWithHttpMessagesAsync(imageStream).Result;
	        return result.Body.AreaOfInterest;
        }


		public void Uninitialize(InitializationEngine context)
        {

        }
    }
}