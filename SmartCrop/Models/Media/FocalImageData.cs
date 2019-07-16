using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
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

        [ScaffoldColumn(false)]
        public virtual int? OriginalWidth { get; set; }

        [ScaffoldColumn(false)]
        public virtual int? OriginalHeight { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            this.SmartCropEnabled = true;
        }
    }
}