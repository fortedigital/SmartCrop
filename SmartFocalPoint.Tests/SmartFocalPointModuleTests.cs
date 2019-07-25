using System.Drawing;
using System.IO;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Forte.SmartFocalPoint;
using Forte.SmartFocalPoint.Business;
using Forte.SmartFocalPoint.Models.Media;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace SmartFocalPointTests
{
    [TestClass]
    public class SmartFocalPointModuleTests
    {

        private readonly Mock<IContentEvents> _contentEvents;
        private readonly Mock<SmartFocalPointAdminPluginSettings> _settingsMock;
        private readonly Mock<CognitiveServicesConnector> _connectorMock;
        private readonly Mock<ModuleUtilities> _utilitiesMock;
        private readonly Mock<SmartFocalPointModule> _moduleMock;
        private Mock<IFocalImageData> _imageMock;

        public SmartFocalPointModuleTests()
        {
            var engineMock = new Mock<InitializationEngine>();
            var locator = new Mock<IServiceLocator>();
            _contentEvents = new Mock<IContentEvents>();

            locator.Setup(x => x.GetInstance<IContentEvents>()).Returns(_contentEvents.Object);
            ServiceLocator.SetLocator(locator.Object);

            _settingsMock = new Mock<SmartFocalPointAdminPluginSettings>();
            _connectorMock = new Mock<CognitiveServicesConnector>();
            _utilitiesMock = new Mock<ModuleUtilities>();

            _moduleMock = new Mock<SmartFocalPointModule>();
            _moduleMock.Object.Initialize(engineMock.Object);
            _moduleMock.SetupGet(x => x.ConnectionSettings).Returns(_settingsMock.Object);
            _moduleMock.SetupGet(x => x.ServicesConnector).Returns(_connectorMock.Object);
            _moduleMock.SetupGet(x => x.Utilities).Returns(_utilitiesMock.Object);

            _imageMock = new Mock<IFocalImageData>();
            _imageMock.Setup(x => x.ContentLink).Returns(new ContentReference(1));
        }
        
        [TestMethod]
        public void ModuleTestWithFocalPointSet()
        {
            var focalPoint = new FocalPoint();
            _imageMock.Setup(x => x.FocalPoint).Returns(focalPoint);

            _contentEvents.Raise(x => x.PublishingContent += null, new ContentEventArgs(_imageMock.Object));
            var actual = _imageMock.Object.FocalPoint;

            _moduleMock.Verify(x => x.Utilities.IsLastVersionFocalPointNull(It.IsAny<IFocalImageData>()), Times.Never);
            Assert.AreSame(focalPoint, actual);
        }

        [TestMethod]
        public void ModuleTestWithLastVersionSet()
        {
            _imageMock.Setup(x => x.FocalPoint).Returns((FocalPoint)null);
            _utilitiesMock.Setup(x => x.IsLastVersionFocalPointNull(It.IsAny<IFocalImageData>())).Returns(false);

            _contentEvents.Raise(x => x.PublishingContent += null, new ContentEventArgs(_imageMock.Object));

            _moduleMock.Verify(x => x.ConnectionSettings.IsConnectionEnabled(), Times.Never);
        }

        [TestMethod]
        public void ModuleTestWithClosedConnection()
        {
            _imageMock.Setup(x => x.FocalPoint).Returns((FocalPoint)null);
            _utilitiesMock.Setup(x => x.IsLastVersionFocalPointNull(It.IsAny<IFocalImageData>())).Returns(true);
            _settingsMock.Setup(x => x.IsConnectionEnabled()).Returns(false);

            _contentEvents.Raise(x => x.PublishingContent += null, new ContentEventArgs(_imageMock.Object));

            _moduleMock.Verify(x => x.Utilities.GetBlobStream(It.IsAny<IBinaryStorable>()), Times.Never);
        }

        [TestMethod]
        public void ModuleTestFailedAreaOfInterest()
        {
            var wasSet = false;
            using (var testStream = new MemoryStream(File.ReadAllBytes("TestData/images.jpg")))
            {
                _imageMock.Setup(x => x.FocalPoint).Returns((FocalPoint)null);
                _imageMock.SetupSet(x => x.FocalPoint = It.IsAny<FocalPoint>()).Callback(() => wasSet = true);
                _utilitiesMock.Setup(x => x.IsLastVersionFocalPointNull(It.IsAny<IFocalImageData>())).Returns(true);
                _settingsMock.Setup(x => x.IsConnectionEnabled()).Returns(true);
                _utilitiesMock.Setup(x => x.GetBlobStream(It.IsAny<IBinaryStorable>())).Returns(testStream);
                _utilitiesMock.Setup(x => x.ResizeImage(It.IsAny<Image>(), It.IsAny<int>()))
                    .Returns(Image.FromStream(testStream));
                _connectorMock.Setup(x => x.GetAreaOfInterest(It.IsAny<Image>())).Returns((BoundingRect) null);

                _contentEvents.Raise(x => x.PublishingContent += null, new ContentEventArgs(_imageMock.Object));
            }

            Assert.IsFalse(wasSet);
        }

        [TestMethod]
        public void ModuleTestValidAreaOfInterest()
        {
            var boundingRect = new BoundingRect(0, 0, 100, 100);
            var wasSet = false;
            using (var testStream = new MemoryStream(File.ReadAllBytes("TestData/images.jpg")))
            {
                _imageMock.SetupSet(x => x.FocalPoint = It.IsAny<FocalPoint>()).Callback(() => wasSet = true);
                _utilitiesMock.Setup(x => x.IsLastVersionFocalPointNull(It.IsAny<IFocalImageData>())).Returns(true);
                _settingsMock.Setup(x => x.IsConnectionEnabled()).Returns(true);
                _utilitiesMock.Setup(x => x.GetBlobStream(It.IsAny<IBinaryStorable>())).Returns(testStream);
                _utilitiesMock.Setup(x => x.ResizeImage(It.IsAny<Image>(), It.IsAny<int>()))
                    .Returns(Image.FromStream(testStream));
                _connectorMock.Setup(x => x.GetAreaOfInterest(It.IsAny<Image>())).Returns(boundingRect);

                _contentEvents.Raise(x => x.PublishingContent += null, new ContentEventArgs(_imageMock.Object));
            }

            Assert.IsTrue(wasSet);
        }
        
    }
}
