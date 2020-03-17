using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NLib.AtpNetCore.Mvc.ModelBinding.RegularExpressions
{
    public static class RegularExpressionExtantions
    {
        public static IEnumerable<Capture> AsEnumerable(this CaptureCollection captureCollection)
        {
            foreach (Capture item in captureCollection)
            {
                yield return item;
            }
        }

        public static IEnumerable<Match> AsEnumerable(this MatchCollection matchCollection)
        {
            foreach (Match item in matchCollection)
            {
                yield return item;
            }
        }
    }
}
