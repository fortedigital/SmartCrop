using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;
using System.ComponentModel.DataAnnotations;

namespace Forte.SmartFocalPoint.Models.Media
{
    public abstract class FocalImageData : ImageData, IFocalImageData
    {
        [Display(Name = "Use FocalPoint for cropping")]
        public virtual bool SmartFocalPointEnabled { get; set; }

        [BackingType(typeof(PropertyFocalPoint))]
        public virtual FocalPoint FocalPoint { get; set; }

        [ScaffoldColumn(false)]
        public virtual int? OriginalWidth { get; set; }

        [ScaffoldColumn(false)]
        public virtual int? OriginalHeight { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            this.SmartFocalPointEnabled = true;
        }
    }
}