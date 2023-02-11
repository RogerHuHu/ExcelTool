using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HIIUtils.String
{
    public static class StringHelper
    {
        /// <summary>
        /// 按行分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> SplitByLine(string str)
        {
            return str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static List<string> Split(string str, params char[] separaters)
        {
            return str.Split(separaters).ToList();
        }

        public static List<string> Split(string str, string[] separaters)
        {
            return str.Split(separaters, StringSplitOptions.None).ToList();
        }

        /// <summary>
        /// 使用正则表达式分割字符串
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <param name="pattern">正则表达式匹配模式</param>
        /// <param name="options">正则表达式选项</param>
        /// <returns></returns>
        public static List<string> Split(string str, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
        {
            return Regex.Split(str, pattern, options).ToList();
        }
    }
}