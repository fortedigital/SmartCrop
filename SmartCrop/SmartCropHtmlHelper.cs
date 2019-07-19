using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Forte.SmartCrop.Models.Media;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using ImageResizer;

namespace Forte.SmartCrop
{
    public static class SmartCropHtmlHelper
    {

        public static MvcHtmlString FocusedImage(
            this HtmlHelper helper,
            ContentReference image,
            bool smart,
            int? width = null,
            int? height = null,
            bool forceSize = false,
            bool noZoomOut = false)
        {
            if (ContentReference.IsNullOrEmpty(image))
            {
                return MvcHtmlString.Empty;
            }

            string imageBaseUrl = ResolveImageUrl(image);
            ServiceLocator.Current.GetInstance<IContentLoader>().TryGet(image, out FocalImageData imageFile);

            if (imageFile.OriginalWidth == null || imageFile.OriginalHeight == null)
            {
                return MvcHtmlString.Empty;
            }

            var parameters = new List<string>();
            //var hasSmartCrop = imageFile.SmartCropEnabled;
            var hasSmartCrop = smart;

            var maxWidth = imageFile.OriginalWidth.Value;
            var maxHeight = imageFile.OriginalHeight.Value;

            //forcing size doesnt make sense if image is big enough
            forceSize = forceSize && (width > maxWidth || height > maxHeight);

            if (noZoomOut && 
                (width == null || width <= maxWidth) &&
                (height == null || height <= maxHeight))
            {
                parameters.Add("crop="+ CalculateCrop(imageFile, 
                                   width ?? maxWidth, 
                                   height ?? maxHeight));
            }
            else
            {
                if (width != null)
                {
                    parameters.Add(
                        hasSmartCrop ? "w=" + width : "width=" + width
                    );
                }

                if (height != null)
                {
                    parameters.Add(
                        hasSmartCrop ? "h=" + height : "height=" + height
                    );
                }
            }

            parameters.Add("mode=crop");

            //forcing size wont do anything with width and height parameters
            if (forceSize && hasSmartCrop)
            {
                parameters.Add("scale=both");
            }

            var separator = imageBaseUrl.Contains("?") ? "&" : "?";
            var imageUrl = imageBaseUrl + separator + string.Join("&", parameters);

            TagBuilder tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes.Add("src", imageUrl);
            return new MvcHtmlString(tagBuilder.ToString());
        }

        private static string CalculateCrop(FocalImageData image, int width, int height)
        {
            var middleX = image.FocalPoint.X * image.OriginalWidth / 100;
            var middleY = image.FocalPoint.Y * image.OriginalHeight / 100;

            var X1 = middleX - width / 2;
            var X2 = middleX + width / 2;
            var Y1 = middleY - height / 2;
            var Y2 = middleY + height / 2;

            if (X1 < 0.0)
            {
                var offset = 0.0 - X1;
                X1 = 0.0;
                X2 += offset;
            }
            if (X2 > image.OriginalWidth)
            {
                var offset = X2 - image.OriginalWidth;
                X1 -= offset;
                X2 = image.OriginalWidth;
            }
            if (Y1 < 0.0)
            {
                var offset = 0.0 - Y1;
                Y1 = 0.0;
                Y2 += offset;
            }
            if (Y2 > image.OriginalHeight)
            {
                var offset = Y2 - image.OriginalHeight;
                Y1 -= offset;
                Y2 = image.OriginalHeight;
            }

            return $"({X1},{Y1},{X2},{Y2})";
        }

        private static string ResolveImageUrl(ContentReference image)
        {
            return UrlResolver.Current.GetUrl(image);
        }

        [Obsolete("This extension is deprecated, use FocusedImage instead")]
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
            ServiceLocator.Current.GetInstance<IContentLoader>().TryGet(image, out FocalImageData imageFile);

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

        private static string CalculateCropBounds(FocalImageData imageFile, int width, int height)
        {
            using (var stream = ReadBlob(imageFile))
            {
                var originalImage = Image.FromStream(stream);

                double cropRatio = width / (double)height;
                double originalRatio = originalImage.Width / (double)originalImage.Height;

                var cropQuery = string.Empty;
                var cropX = 0.0;
                var cropY = 0.0;
                if (cropRatio < originalRatio)
                {
                    var boundingRectHeight = originalImage.Height;
                    var boundingRectWidth = boundingRectHeight * cropRatio;
                    var middlePointX = imageFile.FocalPoint.X * originalImage.Width / 100;

                    cropX = middlePointX - boundingRectWidth / 2;
                    if (cropX < 0)
                        cropX = 0;

                    if (cropX + boundingRectWidth > originalImage.Width)
                        cropX = originalImage.Width - boundingRectWidth;

                    cropQuery = $"{cropX},{cropY},{cropX + boundingRectWidth},{cropY + boundingRectHeight}";
                }
                else
                {
                    var boundingRectWidth = originalImage.Width;
                    var boundingRectHeight = boundingRectWidth / cropRatio;
                    var middlePointY = imageFile.FocalPoint.Y * originalImage.Height / 100;

                    cropY = middlePointY - boundingRectHeight / 2;
                    if (cropY < 0)
                        cropY = 0;

                    if (cropY + boundingRectHeight > originalImage.Height)
                        cropY = originalImage.Height - boundingRectHeight;

                    cropQuery = $"{cropX},{cropY},{cropX + boundingRectWidth},{cropY + boundingRectHeight}";
                }

                //cropQuery += cropX < width ? $"&width={width + cropX}" : $"&width={width}";
                //cropQuery += cropY < height ? $"&height={height + cropY}" : $"&height={height}";

                return cropQuery;
            }
        }

        private static MemoryStream ReadBlob(FocalImageData content)
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
        public RectangleF CalculateCrop(SizeF imageSize, RectangleF areaOfInterests, SizeF cropSize)
        {
            double cropRatio = cropSize.Width / (double)cropSize.Height;
            double originalRatio = imageSize.Width / (double)imageSize.Height;
            
            var cropX = 0.0;
            var cropY = 0.0;
            var boundingRectHeight = 0.0;
            var boundingRectWidth = 0.0;
            if (cropRatio < originalRatio)
            {
                boundingRectHeight = imageSize.Height;
                boundingRectWidth = boundingRectHeight * cropRatio;

                var xFocalPoint = areaOfInterests.Width / 2 + areaOfInterests.X;
                cropX = xFocalPoint - boundingRectWidth / 2;
                if (cropX < 0)
                    cropX = 0;

                if (cropX + boundingRectWidth > imageSize.Width)
                    cropX = imageSize.Width - boundingRectWidth;
            }
            else
            {
                boundingRectWidth = imageSize.Width;
                boundingRectHeight = boundingRectWidth / cropRatio;

                var yFocalPoint = areaOfInterests.Height / 2 + areaOfInterests.Y;
                cropY = yFocalPoint - boundingRectHeight / 2;
                if (cropY < 0)
                    cropY = 0;

                if (cropY + boundingRectHeight > imageSize.Height)
                    cropY = imageSize.Height - boundingRectHeight;
            }
            return new RectangleF((float)cropX, (float)cropY, (float)boundingRectWidth, (float)boundingRectHeight);
        }

    }
}

