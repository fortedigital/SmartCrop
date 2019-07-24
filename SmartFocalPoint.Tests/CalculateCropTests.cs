using Forte.SmartFocalPoint;
using Forte.SmartFocalPoint.Models.Media;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SmartFocalPointTests
{
    [TestClass]
    public class CalculateCropTests
    {
        
        [DataTestMethod]
        [DynamicData(nameof(CalculateCropData.GetData), 
            typeof(CalculateCropData),
                                DynamicDataSourceType.Method)]
        public void CalculateCropTestWithData(FocalPoint focalPoint, int? originalW, int? originalH, 
                                      int width, int height, string expected)
        {
            var mock = new Mock<IFocalImageData>();
            mock.Setup(i => i.FocalPoint).Returns(focalPoint);
            mock.Setup(i => i.OriginalWidth).Returns(originalW);
            mock.Setup(i => i.OriginalHeight).Returns(originalH);

            var actual = SmartFocalPointHtmlHelper.CalculateCrop(mock.Object, width, height);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateCropTestImageIsNull()
        {
            const int cropWidth = 500;
            const int cropHeight = 800;

            var expected = $"({0.0},{0.0},{cropWidth},{cropHeight})";
            var actual = SmartFocalPointHtmlHelper.CalculateCrop(null, cropWidth, cropHeight);

            Assert.AreEqual(expected, actual);
        }

    }
}
