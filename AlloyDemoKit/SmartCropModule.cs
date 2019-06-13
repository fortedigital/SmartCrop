using System;
using System.Drawing;
using System.Drawing.Imaging;
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
	    private const int MaxSize = 1024;

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
	                    var originalImage = Image.FromStream(stream);
	                  
	                    var resizedImage = ResizeImage(originalImage, MaxSize);

						var boundingRect = GetAreaOfInterest(resizedImage);

						double scaleX = 1.0 / (resizedImage.Width  / (double)originalImage.Width);
						double scaleY = 1.0 / (resizedImage.Height / (double)originalImage.Height);

						imageFile.AreaOfInterestX = (int)(boundingRect.X * scaleX);
						imageFile.AreaOfInterestY = (int)(boundingRect.Y * scaleY);
						imageFile.AreaOfInterestWidth = (int)(boundingRect.W * scaleX);
						imageFile.AreaOfInterestHeight = (int)(boundingRect.H * scaleY);
					}
                }
            }

        }

        private static Image ResizeImage(Image originalImage, int maxSize)
        {
	        int w;
	        int h;
	        int desWidth = maxSize;
	        int desHeight = maxSize;

			if (originalImage.Height > originalImage.Width)
	        {
		        w = (originalImage.Width * desHeight) / originalImage.Height;
		        h = desHeight;
		    
	        }
	        else
	        {
		        w = desWidth;
		        h = (originalImage.Height * desWidth) / originalImage.Width;
		       
	        }

			return originalImage.GetThumbnailImage(w, h, null, IntPtr.Zero);
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

        private static BoundingRect GetAreaOfInterest(Image image)
        {
	        using (MemoryStream imageStream = new MemoryStream())
	        {
				image.Save(imageStream, ImageFormat.Png);
				imageStream.Seek(0L, SeekOrigin.Begin);

				var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(ApiKey))
		        {
			        Endpoint = "https://westcentralus.api.cognitive.microsoft.com"
		        };

		        var result = client.GetAreaOfInterestInStreamWithHttpMessagesAsync(imageStream).Result;
		        return result.Body.AreaOfInterest;
	        }
        }


		public void Uninitialize(InitializationEngine context)
        {

        }
    }
}