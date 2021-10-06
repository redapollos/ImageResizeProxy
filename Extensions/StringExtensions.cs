using System;
using System.Collections.Generic;
using System.Text;

namespace RainstormTech.Storm.ImageProxy.Extensions
{
    internal static class StringExtensions
    {
        internal static int ToInt(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return default(int);

            if (int.TryParse(s, out var i))
                return i;

            return default(int);
        }

        internal static string ToSuffix(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return s.Substring(s.LastIndexOf(".") + 1);
        }
    }
}
