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
            //smartFP flag, original width, height (for image mock)
            //width, height, fitmode, noZoom (for helper)
            //expected output parameters
            yield return new object[] {true, 800, 600, null, null, "crop", false, "mode=crop"};
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data == null)
                return null;

            return String.Empty;
        }
    }
}
