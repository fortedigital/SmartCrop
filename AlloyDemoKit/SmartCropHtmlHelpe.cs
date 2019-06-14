using System;
using System.Collections.Generic;
using System.Drawing;
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
                parameters.Add("crop=" + CalculateCropBounds(imageFile, width, height));
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

        private static string CalculateCropBounds(ImageFile imageFile, int? width, int? height)
        {
            int x = imageFile.AreaOfInterestX;
            int y = imageFile.AreaOfInterestY;
            int w = imageFile.AreaOfInterestWidth;
            int h = imageFile.AreaOfInterestHeight;

            return $"{x},{y},{w},{h}";
        }

        private static string ResolveImageUrl(ContentReference image)
        {
            return UrlResolver.Current.GetUrl(image);
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

