using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Forte.SmartFocalPoint;
using Forte.SmartFocalPoint.Models.Media;
using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;
using Moq;

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
            string actual = SmartFocalPointCalculator
                .CalculateCrop(focalX, focalY, originalW, originalH, width, height);

            Assert.AreEqual(expected,actual);
        }

    }
}
