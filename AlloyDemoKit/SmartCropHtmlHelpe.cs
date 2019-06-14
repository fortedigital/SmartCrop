using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AlloyDemoKit.Models.Media;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using ImageResizer.Plugins.EPiServer;

namespace AlloyDemoKit
{
    public static class SmartCropHtmlHelper
    {
        public static MvcHtmlString ResizedPicture(
            this HtmlHelper helper,
            ContentReference image,
            int? width,
            int? height,
            bool smartCrop)
        {
            if (ContentReference.IsNullOrEmpty(image))
            {
                return MvcHtmlString.Empty;
            }
            string imageBaseUrl = ResolveImageUrl(image);
            ServiceLocator.Current.GetInstance<IContentLoader>().TryGet(image, out ImageFile imageFile);

            var isCrop = width != null && height != null;

            var parameters = new List<string>();

            if (smartCrop && isCrop)
            {
                parameters.Add("crop=" + CalculateCropBounds(imageFile, width.Value, height.Value));
            }

            if (width != null)
            {
                parameters.Add("width=" + width.ToString());
            }
            if (height != null)
            {
                parameters.Add("height=" + height.ToString());
            }
            if (isCrop)
            {
                parameters.Add("mode=crop");
            }


            var separator = imageBaseUrl.Contains("?") ? "&" : "?";

            var imageUrl = imageBaseUrl + separator + string.Join("&", parameters);


            TagBuilder tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes.Add("src", imageUrl);

            return new MvcHtmlString(tagBuilder.ToString());

        }

        private static string CalculateCropBounds(ImageFile imageFile, int width, int height)
        {
	        using (var stream = ReadBlob(imageFile))
	        {
		        var originalImage = Image.FromStream(stream);
		     

		        var areaX = imageFile.AreaOfInterestX;
		        var areaY = imageFile.AreaOfInterestY;
				var areaW = imageFile.AreaOfInterestWidth;
				var areaH = imageFile.AreaOfInterestHeight;


				double cropRatio = width / (double)height;
				double originalRatio = originalImage.Width / (double)originalImage.Height;

				var cropQuery = string.Empty;
				if (cropRatio < originalRatio)
				{
					var boundingRectHeight = originalImage.Height;
					var boundingRectWidth = boundingRectHeight * cropRatio;

					var xFocalPoint = areaW / 2 + areaX;
					var cropX = xFocalPoint - boundingRectWidth / 2;
					if (cropX < 0)
						cropX = 0;

					if (cropX + boundingRectWidth > originalImage.Width)
						cropX = originalImage.Width - boundingRectWidth;

					var cropY = 0.0;

					cropQuery = $"{cropX},{cropY},{cropX + boundingRectWidth},{cropY + boundingRectHeight}";
				}
				else
				{
					var boundingRectWidth = originalImage.Width;
					var boundingRectHeight = boundingRectWidth / cropRatio;



					var yFocalPoint = areaH / 2 + areaY;
					var cropY = yFocalPoint - boundingRectHeight / 2;
					if (cropY < 0)
						cropY = 0;

					if (cropY + boundingRectHeight > originalImage.Height)
						cropY = originalImage.Height - boundingRectHeight;

					var cropX = 0.0;

					cropQuery = $"{cropX},{cropY},{cropX + boundingRectWidth},{cropY + boundingRectHeight}";
				}

				return cropQuery;
	        }
        }

        private static string ResolveImageUrl(ContentReference image)
        {
            return UrlResolver.Current.GetUrl(image);
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
	}



    public class SmartCropCalculator
    {
        public Rectangle CalculateCrop(Size imageSize, Rectangle areaOfInterests, Size cropSize)
        {
            throw new NotImplementedException();
        }

    }
}

