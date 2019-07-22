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
        public void CalculateCropTest()
        {
            string expected = $"({750.0},{600.0},{1050.0},{900.0})";
            string actual = SmartFocalPointCalculator
                .CalculateCrop(50.0, 50.0, 1800, 1500, 300, 300);

            Assert.AreEqual(expected,actual);
        }

    }
}
