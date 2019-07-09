using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCrop;

namespace SmartCrop.Tests
{
    [TestClass]
    public class SmartCropTests
    {
        [TestMethod]
        public void CropLeftWithoutResize()
        {
            SmartCropCalculator calculator=new SmartCropCalculator();
            var crop = calculator.CalculateCrop(new Size(100, 50), new Rectangle(0, 0, 50, 50), new Size(50, 50));

            Assert.AreEqual(crop,new Rectangle(0,0,50,50));
        }

        [TestMethod]
        public void CropBottom()
        {
            SmartCropCalculator calculator = new SmartCropCalculator();
            var crop = calculator.CalculateCrop(new Size(50, 100), new Rectangle(0, 50, 50, 50), new Size(50, 50));

            Assert.AreEqual(crop, new Rectangle(0, 0, 50, 50));
        }
    }
}
