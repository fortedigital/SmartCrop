using ImageResizer.Plugins.EPiFocalPoint;

namespace Forte.SmartFocalPoint.Models.Media
{
    public interface IFocalImageData : IFocalPointData
    {

        bool SmartFocalPointEnabled { get; set; }

    }
}
