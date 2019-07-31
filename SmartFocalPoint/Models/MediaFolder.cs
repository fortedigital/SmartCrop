using System;
using EPiServer.Core;
using System.Collections.Generic;

namespace Forte.SmartFocalPoint.Models
{
    public class MediaFolder
    {
        public ContentReference FolderReference { get; set; }
        public string FolderName { get; set; }
        public Guid FolderGuid { get; set; }
        public List<MediaFolder> ChildrenFolders { get; set; }

        public MediaFolder(ContentReference reference, Guid folderGuid) : this(reference, folderGuid, "Root") { }

        public MediaFolder(ContentReference reference, Guid folderGuid, string folderName)
        {
            this.FolderReference = reference;
            this.FolderName = folderName;
            this.FolderGuid = folderGuid;
            this.ChildrenFolders = new List<MediaFolder>();
        }

        public void AddChildFolder(ContentReference childReference, Guid childGuid, string childName)
        {
            this.ChildrenFolders.Add(new MediaFolder(childReference, childGuid, childName));
        }

    }
}
