using System;

namespace Forte.SmartFocalPoint.Models.ViewModels
{
    public class SmartFocalPointAdminPluginViewModel
    {
        public bool IsSmartFocalPointEnabled { get; set; }

        public Guid ChosenFolderGuid { get; set; }

        public MediaFolder MediaFolder { get; set; }
    }
}
