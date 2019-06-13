using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;

namespace AlloyDemoKit.Models.Media
{
    [ContentType(GUID = "0A89E464-56D4-449F-AEA8-2BF774AB8730")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
    public class ImageFile : ImageData, IFileProperties
    {
        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public virtual string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the alt tag.
        /// </summary>
        /// <value>
        /// The alt tag.
        /// </value>
        public virtual string AltTag { get; set; }

        [Editable(false)]
        public virtual string FileSize { get; set; }

        public virtual int AreaOfInterestX { get; set; }
        public virtual int AreaOfInterestY { get; set; }
        public virtual int AreaOfInterestWidth { get; set; }
        public virtual int AreaOfInterestHeight { get; set; }

        public virtual bool SmartCropEnabled { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            this.SmartCropEnabled = true;
        }
    }

}
