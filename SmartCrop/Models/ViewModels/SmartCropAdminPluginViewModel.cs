using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;

namespace Forte.SmartCrop.Models.ViewModels
{
    public class SmartCropAdminPluginViewModel
    {
        public IEnumerable<ImageData> ImagesEnumerable { get; set; }
        
    }
}
