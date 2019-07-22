using System.Drawing;
using Forte.SmartFocalPoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Forte.SmartFocalPoint.Tests
{
    [TestClass]
    public class SmartFocalPointTests
    {
        [TestMethod]
        public void CropLeftWithoutResize()
        {
            SmartFocalPointCalculator calculator = new SmartFocalPointCalculator();
            var crop = calculator.CalculateCrop(new Size(100, 50), new Rectangle(0, 0, 50, 50), new Size(50, 50));

            Assert.AreEqual(new Rectangle(0,0,50,50), crop);
        }

        [TestMethod]
        public void CropBottom()
        {
            SmartFocalPointCalculator calculator = new SmartFocalPointCalculator();
            var crop = calculator.CalculateCrop(new Size(50, 100), new Rectangle(0, 50, 50, 50), new Size(50, 50));

            Assert.AreEqual( new Rectangle(0, 50, 50, 50), crop);
        }
    }
}
