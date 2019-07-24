using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Forte.SmartFocalPoint.Models.Media;
using System;
using System.Collections.Generic;
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
            var imageFile = ServiceLocator.Current.GetInstance<IContentLoader>().Get<IFocalImageData>(image);

            if (imageFile?.OriginalWidth == null || imageFile.OriginalHeight == null)
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

            var tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes.Add("src", imageUrl);
            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static string CalculateCrop(IFocalImageData image, int width, int height)
        {
            if (image?.OriginalWidth == null || image.OriginalHeight == null)
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

    }

}

