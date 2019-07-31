using EPiServer.Core;
using System.Collections.Generic;

namespace Forte.SmartFocalPoint.Models
{
    public class MediaFolder
    {
        public ContentReference FolderReference { get; set; }

        public List<MediaFolder> ChildrenFolders { get; set; }

        public MediaFolder(ContentReference reference)
        {
            this.FolderReference = reference;
            this.ChildrenFolders = new List<MediaFolder>();
        }

        public void AddChildFolder(ContentReference childReference)
        {
            this.ChildrenFolders.Add(new MediaFolder(childReference));
        }

    }
}
