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

        private int _focalPointX;
        private int _focalPointY;
        private FocalPoint _focalPoint;

        public virtual int FocalPointX
        {
            get => _focalPointX;
            set
            {
                _focalPointX = value;
                if (_focalPoint == null) return;
                if (OriginalWidth != null) _focalPoint.X = 100 * _focalPointX / (double) OriginalWidth;
            }
        }

        public virtual int FocalPointY
        {
            get => _focalPointY;
            set
            {
                _focalPointY = value;
                if (_focalPoint == null) return;
                if (OriginalHeight != null) _focalPoint.Y = 100 * _focalPointY / (double) OriginalHeight;
            }
        }

        public virtual bool SmartCropEnabled { get; set; }

        [BackingType(typeof(PropertyFocalPoint))]
        public virtual FocalPoint FocalPoint
        {
            get => _focalPoint;
            set
            {
                _focalPoint = value;
                if (OriginalWidth != null) _focalPointX = (int)((int) OriginalWidth * _focalPoint.X / 100);
                if (OriginalHeight != null) _focalPointY = (int)((int) OriginalHeight * _focalPoint.Y / 100);
            }
        }

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