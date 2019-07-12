using EPiServer.Core;

namespace Forte.SmartCrop.Models.Media
{
    public abstract class FocalImageData : ImageData
    {
        
        public virtual int FocalPointX { get; set; }

        public virtual int FocalPointY { get; set; }

        public virtual bool SmartCropEnabled { get; set; }
    }
}