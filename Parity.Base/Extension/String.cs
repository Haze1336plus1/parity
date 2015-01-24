using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExt
    {

        public static string Fallback(this string @this, string @default = "")
        {
            return (@this == null || @this.Trim().Length == 0) ? @default : @this;
        }

        public static string Process(this string @this, params object[] arguments)
        {
            return String.Format(@this, arguments);
        }
    }
}
