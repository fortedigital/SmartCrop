using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.MirroringService.Common;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Forte.SmartFocalPoint;
using Forte.SmartFocalPoint.Models.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SmartFocalPointTests
{
    /// <summary>
    /// Summary description for FocusedImageHelperTests
    /// </summary>
    [TestClass]
    public class FocusedImageHelperTests
    {
        private static HtmlHelper _helper;

        [ClassInitialize]
        public static void ClassInitializer(TestContext testContext)
        {
            var viewContextMock = new Mock<ViewContext>();
            var viewDataContainerMock = new Mock<IViewDataContainer>();
            _helper = new HtmlHelper(viewContextMock.Object, viewDataContainerMock.Object);
        }

        [TestMethod]
        public void FocusedImageTestWithNullReference()
        {
            var expected = MvcHtmlString.Empty;
            var actual = _helper.FocusedImage(null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FocusedImageTest()
        {
            var cref = new ContentReference(1);
            var imageMock = new Mock<IFocalImageData>();
            var repo = new Mock<IContentRepository>();
            repo.Setup(x => x.Get<IFocalImageData>(It.IsAny<ContentReference>())).Returns(imageMock.Object);
            var resolver = new Mock<UrlResolver>();
            resolver.Setup(x => x.GetUrl(cref)).Returns("fake");
            var loader = new Mock<IContentLoader>();
            loader.Setup(x => x.Get<IFocalImageData>(cref)).Returns(imageMock.Object);
            

            var locator = new Mock<IServiceLocator>();
            locator.Setup(x => x.GetInstance<UrlResolver>()).Returns(resolver.Object);
            locator.Setup(x => x.GetInstance<IContentRepository>()).Returns(repo.Object);
            locator.Setup(x => x.GetInstance<IContentLoader>()).Returns(loader.Object);
            ServiceLocator.SetLocator(locator.Object);

            _helper.FocusedImage(cref);

            Assert.IsTrue(true);
        }
    }
}
