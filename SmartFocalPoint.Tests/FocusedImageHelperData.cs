using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartFocalPointTests
{
    public class FocusedImageHelperDataAttribute : Attribute, ITestDataSource
    {

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            //argument order:
            //smartFP flag (for image mock)
            //width, height, fitmode, noZoom (for helper)
            //expected output parameters
            //Test data:
            //if all parameters are default
            yield return new object[] { true, null, null, "crop", false, "mode=crop"};
            //width/height testing with smart flag
            yield return new object[] { true, 200, null, "crop", false, "w=200&amp;mode=crop"};
            yield return new object[] { true, null, 200, "crop", false, "h=200&amp;mode=crop" };
            yield return new object[] { true, 200, 100, "crop", false, "w=200&amp;h=100&amp;mode=crop" };
            //width/height testing without smart flag
            yield return new object[] { false, 200, null, "crop", false, "width=200&amp;mode=crop" };
            yield return new object[] { false, null, 200, "crop", false, "height=200&amp;mode=crop" };
            yield return new object[] { false, 200, 100, "crop", false, "width=200&amp;height=100&amp;mode=crop" };
            //zoom testing
            yield return new object[] { true, 200, 200, "crop", true, $"crop=({300.0},{200.0},{500.0},{400.0})&amp;mode=crop" };
            yield return new object[] { true, 1000, 200, "crop", true, "w=1000&amp;h=200&amp;mode=crop" };
            //modes testing
            yield return new object[] { false, 100, 200, "fill", false, "width=100&amp;height=200" };
            yield return new object[] { true, 100, 200, "fill", false, "w=100&amp;h=200" };
            yield return new object[] { true, 1000, 200, "fill", false, "w=1000&amp;h=200&amp;scale=both" };
            yield return new object[] { true, 100, 200, "contain", false, "w=100&amp;h=200&amp;mode=max" };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data == null)
                return null;

            return $"{methodInfo.Name} - Input: FocalImageData(SmartFocalPoint:{data[0]}) " +
                   $"Parameters({data[1]},{data[2]},mode: {data[3]},zoom: {data[4]}) " +
                   $"Output: {data[5]}";
        }
    }
}
