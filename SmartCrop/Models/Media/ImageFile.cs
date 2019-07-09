using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.DataAbstraction;

namespace SmartCrop.Models.Media
{
    [ContentType(GUID = "0A89E464-56D4-449F-AEA8-2BF774AB8730")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
    public class ImageFile : ImageData
    {
        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public virtual string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the top-left X point coordinate of area of interest
        /// </summary>
        /// <value>
        /// Top-left X point coordinate of area of interest
        /// </value>
        public virtual int AreaOfInterestX { get; set; }

        /// <summary>
        /// Gets or sets the top-left Y point coordinate of area of interest
        /// </summary>
        /// <value>
        /// Top-left Y point coordinate of area of interest
        /// </value>
        public virtual int AreaOfInterestY { get; set; }

        /// <summary>
        /// Gets or sets the width of area of interest
        /// </summary>
        /// <value>
        /// The width of area of interest
        /// </value>
        public virtual int AreaOfInterestWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of area of interest
        /// </summary>
        /// <value>
        /// The height of area of interest
        /// </value>
        public virtual int AreaOfInterestHeight { get; set; }

        /// <summary>
        /// Gets or sets the smart crop flag
        /// </summary>
        /// <value>
        /// Flag telling whether smart crop is enabled for this model
        /// </value>
        public virtual bool SmartCropEnabled { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            this.SmartCropEnabled = true;
        }
    }

}
