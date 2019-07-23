using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartFocalPointTests
{
    public class CropDataSourceAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            //format: focalX, focalY, originalWidth, originalHeight, width, height,
            //(expected): X1, Y1, X2, Y2
            //if area is in the middle
            yield return new object[] {50.0, 50.0, 1800, 1500, 300, 300, $"({750.0},{600.0},{1050.0},{900.0})"};
            //if area is touching bounds
            yield return new object[] {10.0, 10.0, 1000, 1000, 200, 200, $"({0.0},{0.0},{200.0},{200.0})"};
            yield return new object[] {90.0, 10.0, 1000, 1000, 200, 200, $"({800.0},{0.0},{1000.0},{200.0})"};
            yield return new object[] {10.0, 90.0, 1000, 1000, 200, 200, $"({0.0},{800.0},{200.0},{1000.0})"};
            yield return new object[] {90.0, 90.0, 1000, 1000, 200, 200, $"({800.0},{800.0},{1000.0},{1000.0})"};
            //if area cuts bounds
            yield return new object[] {0.0, 0.0, 800, 600, 200, 150, $"({0.0},{0.0},{200.0},{150.0})" };
            yield return new object[] {100.0, 0.0, 800, 600, 200, 150, $"({600.0},{0.0},{800.0},{150.0})" };
            yield return new object[] {0.0, 100.0, 800, 600, 200, 150, $"({0.0},{450.0},{200.0},{600.0})" };
            yield return new object[] {100.0, 100.0, 800, 600, 200, 150, $"({600.0},{450.0},{800.0},{600.0})" };
            //various unusual sizes
            yield return new object[] { 63.0, 82.0, 1336, 768, 480, 320, $"({601.68},{448.0},{1081.68},{768.0})" };
            yield return new object[] { 39.24, 33.62, 1336, 768, 640, 480, $"({204.2464},{18.2016},{844.2464},{498.2016})" };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null)
            {
                return $"{methodInfo.Name} - Expected: {data[6]}, Input: FocalPoint ({data[0]},{data[1]})" +
                       $" OriginalSize ({data[2]},{data[3]}) CropSize ({data[4]},{data[5]})";
            }

            return null;
        }
    }
}
