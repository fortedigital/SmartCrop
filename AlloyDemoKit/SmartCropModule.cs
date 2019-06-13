using System;
using System.IO;
using AlloyDemoKit.Models.Media;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Shell;

namespace AlloyDemoKit
{
    [ModuleDependency(typeof(ShellInitialization))]
    [InitializableModule]
    public class SmartCropModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            context.Locate.ContentEvents().PublishingContent += new EventHandler<ContentEventArgs>(this.HandlePublishingContent);
        }

        private void HandlePublishingContent(object sender, ContentEventArgs e)
        {
            if (e.Content is ImageFile)
            {
                var imageFile = (ImageFile)e.Content;

                if (imageFile.SmartCropEnabled)
                {
                    using (var stream = ReadBlob(imageFile))
                    {
                        imageFile.AreaOfInterestHeight = 1;
                        imageFile.AreaOfInterestHeight = 2;
                    }
                }
            }

        }

        private static MemoryStream ReadBlob(ImageFile content)
        {
            using (var stream = content.BinaryData.OpenRead())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                var memoryStream = new MemoryStream(buffer, writable: false);
                return memoryStream;
            }
        }


        public void Uninitialize(InitializationEngine context)
        {

        }
    }
}