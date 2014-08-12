using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMZY.Util
{
    public static class StringExtensions
    {
        public static string NullOrEmptyDefault(this string source, string @default)
        {
            return string.IsNullOrEmpty(source) ? @default : source;
        }

        public static string NullOrWhiteSpaceDefault(this string source, string @default)
        {
            return string.IsNullOrWhiteSpace(source) ? @default : source;
        }
    }
}
