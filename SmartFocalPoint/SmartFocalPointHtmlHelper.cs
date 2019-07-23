using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Forte.SmartFocalPoint.Models.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

namespace Forte.SmartFocalPoint
{
    public static class SmartFocalPointHtmlHelper
    {

        public static MvcHtmlString FocusedImage(
            this HtmlHelper helper,
            ContentReference image,
            int? width = null,
            int? height = null,
            string objectFitMode = "crop",
            bool noZoomOut = false)
        {
            if (ContentReference.IsNullOrEmpty(image))
            {
                return MvcHtmlString.Empty;
            }

            var imageBaseUrl = ResolveImageUrl(image);
            ServiceLocator.Current.GetInstance<IContentLoader>().TryGet(image, out FocalImageData imageFile);

            if (imageFile.OriginalWidth == null || imageFile.OriginalHeight == null)
            {
                return MvcHtmlString.Empty;
            }

            var parameters = new List<string>();
            var isSmartFocalPointEnabled = imageFile.SmartFocalPointEnabled;

            var maxWidth = imageFile.OriginalWidth.Value;
            var maxHeight = imageFile.OriginalHeight.Value;
            
            var widthValue = width ?? maxWidth;
            var heightValue = height ?? maxHeight;

            if (noZoomOut && 
                widthValue <= maxWidth && 
                heightValue <= maxHeight)
            {
                parameters.Add("crop="+ CalculateCrop(imageFile, widthValue, heightValue));
            }
            else
            {
                if (width != null)
                {
                    parameters.Add(isSmartFocalPointEnabled ? "w=" + width : "width=" + width);
                }

                if (height != null)
                {
                    parameters.Add(isSmartFocalPointEnabled ? "h=" + height : "height=" + height);
                }
            }

            switch (objectFitMode)
            {
                case "fill":
                    if (isSmartFocalPointEnabled &&
                        (width > maxWidth || height > maxHeight))
                    {
                        parameters.Add("scale=both");
                    }
                    break;
                case "contain":
                    parameters.Add("mode=max");
                    break;
                default:
                    parameters.Add("mode=crop");
                    break;
            }
            
            var separator = imageBaseUrl.Contains("?") ? "&" : "?";
            var imageUrl = imageBaseUrl + separator + string.Join("&", parameters);

            TagBuilder tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes.Add("src", imageUrl);
            return new MvcHtmlString(tagBuilder.ToString());
        }

        private static string CalculateCrop(FocalImageData image, int width, int height)
        {
            if (image == null)
                return $"({0},{0},{width},{height})";

            var x = image.FocalPoint?.X ?? 50.0;
            var y = image.FocalPoint?.Y ?? 50.0;

            var middleX = x * image.OriginalWidth / 100;
            var middleY = y * image.OriginalHeight / 100;

            var X1 = middleX - width / 2;
            var X2 = X1 + width;
            var Y1 = middleY - height / 2;
            var Y2 = Y1 + height;

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

            X1 = Math.Round(X1.Value, 4, MidpointRounding.ToEven);
            X2 = Math.Round(X2.Value, 4, MidpointRounding.ToEven);
            Y1 = Math.Round(Y1.Value, 4, MidpointRounding.ToEven);
            Y2 = Math.Round(Y2.Value, 4, MidpointRounding.ToEven);

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

                if (imageFile.FocalPoint == null)
                {
                    return $"0,0,{originalImage.Width},{originalImage.Height}";
                }

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

    public class SmartFocalPointCalculator
    {
        public static string CalculateCrop(double X, double Y, int? originalWidth, int? originalHeight, int width, int height)
        {
            var middleX = X * originalWidth / 100;
            var middleY = Y * originalHeight / 100;

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
            if (X2 > originalWidth)
            {
                var offset = X2 - originalWidth;
                X1 -= offset;
                X2 = originalWidth;
            }
            if (Y1 < 0.0)
            {
                var offset = 0.0 - Y1;
                Y1 = 0.0;
                Y2 += offset;
            }
            if (Y2 > originalHeight)
            {
                var offset = Y2 - originalHeight;
                Y1 -= offset;
                Y2 = originalHeight;
            }

            return $"({X1},{Y1},{X2},{Y2})";
        }
    }

}

