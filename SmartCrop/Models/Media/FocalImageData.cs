using EPiServer.Core;
using ImageResizer.Plugins.EPiFocalPoint;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;

namespace Forte.SmartCrop.Models.Media
{
    public abstract class FocalImageData : ImageData, IFocalPointData
    {

        public virtual int FocalPointX { get; set; }

        public virtual int FocalPointY { get; set; }

        public virtual bool SmartCropEnabled { get; set; }

        public FocalPoint FocalPoint { get; set; }

        public int? OriginalWidth { get; set; }

        public int? OriginalHeight { get; set; }
    }
}