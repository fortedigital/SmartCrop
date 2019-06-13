using System.Collections.Generic;
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
            ImageFile imageFile;
            ServiceLocator.Current.GetInstance<IContentLoader>().TryGet(image, out imageFile);

            string imageUrl;

            var isCrop = width != null && height != null;
            if (smartCrop==false || isCrop==false)
            {
                var parameters = new Dictionary<string, string>();

                if (width != null)
                {
                    parameters.Add("width", width.ToString());
                }
                if (height != null)
                {
                    parameters.Add("height", height.ToString());
                }
                if (isCrop)
                {
                    parameters.Add("mode", "crop");
                }

                imageUrl = imageBaseUrl + "?" + string.Join("&", parameters.Select(x => x.Key + "=" + x.Value));
            }
            else
            {
               imageUrl=imageBaseUrl+"?crop="+CalculateCropBounds(imageFile, width, height);
            }

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
}
