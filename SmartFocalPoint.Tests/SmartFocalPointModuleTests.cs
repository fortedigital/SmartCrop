using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Forte.SmartFocalPoint;
using Forte.SmartFocalPoint.Models.Media;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace SmartFocalPointTests
{
    [TestClass]
    public class SmartFocalPointModuleTests
    {

        private readonly Mock<IContentEvents> _contentEvents;
        private readonly Mock<IFocalImageData> _imageMock;
        private readonly Mock<SmartFocalPointModule> _moduleMock;

        public SmartFocalPointModuleTests()
        {
            var engineMock = new Mock<InitializationEngine>();
            var locator = new Mock<IServiceLocator>();
            _contentEvents = new Mock<IContentEvents>();

            locator.Setup(x => x.GetInstance<IContentEvents>()).Returns(_contentEvents.Object);
            ServiceLocator.SetLocator(locator.Object);
            _moduleMock = new Mock<SmartFocalPointModule>();
            _moduleMock.Object.Initialize(engineMock.Object);

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

            Assert.AreSame(focalPoint, actual);
        }
        
    }
}
