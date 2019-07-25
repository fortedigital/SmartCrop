using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Forte.SmartFocalPoint.Models.Media;
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
                parameters.Add("crop="+ CropCalculator.CalculateCrop(imageFile, widthValue, heightValue));
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

        private static string ResolveImageUrl(ContentReference image)
        {
            return UrlResolver.Current.GetUrl(image);
        }

    }

}

