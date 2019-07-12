using EPiServer.Core;
using EPiServer.DataAnnotations;
using ImageResizer.Plugins.EPiFocalPoint;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;

namespace Forte.SmartCrop.Models.Media
{
    public abstract class FocalImageData : ImageData, IFocalPointData
    {

        public virtual int FocalPointX { get; set; }

        public virtual int FocalPointY { get; set; }

        public virtual bool SmartCropEnabled { get; set; }

        [BackingType(typeof(PropertyFocalPoint))]
        public virtual FocalPoint FocalPoint { get; set; }

        public virtual int? OriginalWidth { get; set; }

        public virtual int? OriginalHeight { get; set; }
    }
}