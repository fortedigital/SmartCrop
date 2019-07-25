using ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties;
using System.Collections.Generic;

namespace SmartFocalPointTests
{
    public class CropCalculatorData
    {

        public static IEnumerable<object[]> GetInboundsData()
        {
            //format: focalpoint, originalWidth, originalHeight, width, height,
            //(expected): X1, Y1, X2, Y2
            return new List<object[]>
            {
                //if area is in the middle
                new object[]
                    {new FocalPoint {X = 50.0, Y = 50.0}, 1800, 1500, 300, 300, $"({750.0},{600.0},{1050.0},{900.0})"},
                //if area is touching bounds
                new object[]
                    {new FocalPoint {X = 10.0, Y = 10.0}, 1000, 1000, 200, 200, $"({0.0},{0.0},{200.0},{200.0})"},
                new object[]
                    {new FocalPoint {X = 90.0, Y = 10.0}, 1000, 1000, 200, 200, $"({800.0},{0.0},{1000.0},{200.0})"},
                new object[]
                    {new FocalPoint {X = 10.0, Y = 90.0}, 1000, 1000, 200, 200, $"({0.0},{800.0},{200.0},{1000.0})"},
                new object[]
                    {new FocalPoint {X = 90.0, Y = 90.0}, 1000, 1000, 200, 200, $"({800.0},{800.0},{1000.0},{1000.0})"},
                
                //various unusual sizes
                new object[]
                    {new FocalPoint {X = 63.0, Y = 82.0}, 1336, 768, 480, 320, $"({601.68},{448.0},{1081.68},{768.0})"},
                new object[]
                {
                    new FocalPoint {X = 39.24, Y = 33.62}, 1336, 768, 640, 480,
                    $"({204.2464},{18.2016},{844.2464},{498.2016})"
                }
            };
        }

        public static IEnumerable<object[]> GetOutboundsData()
        {
            return new List<object[]>
            {
                //if area cuts bounds
                new object[] {new FocalPoint {X = 0.0, Y = 0.0}, 800, 600, 200, 150, $"({0.0},{0.0},{200.0},{150.0})"},
                new object[]
                    {new FocalPoint {X = 100.0, Y = 0.0}, 800, 600, 200, 150, $"({600.0},{0.0},{800.0},{150.0})"},
                new object[]
                    {new FocalPoint {X = 0.0, Y = 100.0}, 800, 600, 200, 150, $"({0.0},{450.0},{200.0},{600.0})"},
                new object[]
                    {new FocalPoint {X = 100.0, Y = 100.0}, 800, 600, 200, 150, $"({600.0},{450.0},{800.0},{600.0})"}
            };
        }

        public static IEnumerable<object[]> GetWrongData()
        {
            return new List<object[]>
            {
                //if focalpoint is null
                new object[]{null, 1000, 800, 500, 500, $"({250.0},{150.0},{750.0},{650.0})" },
                //if original sizes are null
                new object[]{ new FocalPoint { X = 0.0, Y = 0.0 }, null, 800, 500, 500, $"({0},{0},{500},{500})" },
                new object[]{ new FocalPoint { X = 0.0, Y = 0.0 }, 1000, null, 500, 500, $"({0},{0},{500},{500})" }
            };
        }
        
    }
}
