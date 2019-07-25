using System;
using System.Drawing;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Forte.SmartFocalPoint.Models.Media;
using ImageResizer.Plugins.EPiFocalPoint;
using System.IO;
using System.Linq;

namespace Forte.SmartFocalPoint
{
    public class ModuleUtilities
    {

        public MemoryStream GetBlobStream(IBinaryStorable content)
        {
            using (var stream = content.BinaryData.OpenRead())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                var memoryStream = new MemoryStream(buffer, writable: false);
                return memoryStream;
            }
        }

        public bool IsLastVersionFocalPointNull(IFocalPointData image)
        {
            var contentVersionRepository = ServiceLocator.Current.GetInstance<IContentVersionRepository>();
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

            if (ContentReference.IsNullOrEmpty(image.ContentLink)) return true;

            var lastVersion = contentVersionRepository
                .List(image.ContentLink)
                .Where(p => p.Status == VersionStatus.PreviouslyPublished)
                .OrderByDescending(p => p.Saved)
                .FirstOrDefault();

            if (lastVersion == null) return true;

            var lastImage = contentRepository.Get<IFocalImageData>(lastVersion.ContentLink);
            return lastImage.FocalPoint == null;
        }

        public Image ResizeImage(Image originalImage, int maxSize)
        {
            int w;
            int h;
            var desWidth = maxSize;
            var desHeight = maxSize;

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
    }
}
