using Forte.SmartFocalPoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartFocalPointTests
{
    [TestClass]
    public class CalculateCropTests
    {
        
        [TestMethod]
        [CropDataSource]
        public void CalculateCropTest(double focalX, double focalY, 
                                      int? originalW, int? originalH, 
                                      int width, int height, string expected)
        {
            var actual = SmartFocalPointCalculator
                .CalculateCrop(focalX, focalY, originalW, originalH, width, height);

            Assert.AreEqual(expected,actual);
        }

    }
}
