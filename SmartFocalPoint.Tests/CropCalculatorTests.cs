using Forte.SmartFocalPoint;
using Forte.SmartFocalPoint.Models.Media;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SmartFocalPointTests
{
    [TestClass]
    public class CropCalculatorTests
    {

        private Mock<IFocalImageData> _imageMock = new Mock<IFocalImageData>();
        
        [DataTestMethod]
        [DynamicData(nameof(CropCalculatorData.GetInboundsData), 
            typeof(CropCalculatorData),
                                DynamicDataSourceType.Method)]
        public void CalculateCropTestWithInboundsData(FocalPoint focalPoint, int? originalW, int? originalH, 
                                      int width, int height, string expected)
        {
            SetupImageMock(focalPoint, originalW, originalH);

            var actual = CropCalculator.CalculateCrop(_imageMock.Object, width, height);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(CropCalculatorData.GetOutboundsData),
            typeof(CropCalculatorData),
            DynamicDataSourceType.Method)]
        public void CalculateCropTestWithOutboundsData(FocalPoint focalPoint, int? originalW, int? originalH,
            int width, int height, string expected)
        {
            SetupImageMock(focalPoint, originalW, originalH);

            var actual = CropCalculator.CalculateCrop(_imageMock.Object, width, height);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(CropCalculatorData.GetWrongData),
            typeof(CropCalculatorData),
            DynamicDataSourceType.Method)]
        public void CalculateCropTestWithWrongData(FocalPoint focalPoint, int? originalW, int? originalH,
            int width, int height, string expected)
        {
            SetupImageMock(focalPoint, originalW, originalH);

            var actual = CropCalculator.CalculateCrop(_imageMock.Object, width, height);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateCropTestImageIsNull()
        {
            const int cropWidth = 500;
            const int cropHeight = 800;

            var expected = $"({0.0},{0.0},{cropWidth},{cropHeight})";
            var actual = CropCalculator.CalculateCrop(null, cropWidth, cropHeight);

            Assert.AreEqual(expected, actual);
        }

        private void SetupImageMock(FocalPoint focalPoint, int? originalWidth, int? originalHeight)
        {
            _imageMock.Setup(i => i.FocalPoint).Returns(focalPoint);
            _imageMock.Setup(i => i.OriginalWidth).Returns(originalWidth);
            _imageMock.Setup(i => i.OriginalHeight).Returns(originalHeight);
        }

    }
}
