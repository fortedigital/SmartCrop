using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using EPiServer.Logging;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Forte.SmartFocalPoint
{
    public class CognitiveServicesConnector
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        public BoundingRect GetAreaOfInterest(Image image)
        {
            using (var imageStream = new MemoryStream())
            {
                image.Save(imageStream, ImageFormat.Png);
                imageStream.Seek(0L, SeekOrigin.Begin);

                var key = ConfigurationManager.AppSettings["CognitiveServicesApiKey"];
                var server = ConfigurationManager.AppSettings["CognitiveServicesServer"];

                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                {
                    Endpoint = server
                };
                try
                {
                    var result = client.GetAreaOfInterestInStreamWithHttpMessagesAsync(imageStream).Result;
                    return result.Body.AreaOfInterest;
                }
                catch (AggregateException exceptions)
                {
                    exceptions.Handle(HandleException);
                    return null;
                }
            }
        }

        private static bool HandleException(Exception ex)
        {
            if (!(ex is ComputerVisionErrorException exception)) return false;
            Logger.Error(exception.Body.ToString());
            return true;

        }
    }
}
