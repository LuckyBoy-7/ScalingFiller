using System.Collections.Generic;
using Lucky.Extensions;
using UnityEngine;

namespace Lucky.Utilities
{
    public static class ColorUtils
    {
        private static string pattern = "0123456789ABCDEF";

        private static List<Color> allColors = new List<Color>()
        {
            Color.black,
            Color.blue,
            Color.cyan,
            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,
            Color.white,
            Color.yellow,
        };


        /// <summary>
        /// 返回一个[0, 255]整形对应的16进制字符串
        /// </summary>
        /// <param name="x">[0, 255]</param>
        /// <returns></returns>
        private static string ToHex(int x)
        {
            x = Mathf.Clamp(x, 0, 255);
            return $"{pattern[x / 16]}{pattern[x % 16]}";
        }

        public static string Wrap(string orig, string surround) => $"<color={surround}>{orig}</color>";

        public static string Wrap(string orig, Color color)
        {
            string r = ToHex((int)(color.r * 255));
            string g = ToHex((int)(color.g * 255));
            string b = ToHex((int)(color.b * 255));
            string a = ToHex((int)(color.a * 255));
            return $"<color=#{r}{g}{b}{a}>{orig}</color>";
        }

        public static Color GetRandomColor() => allColors.Choice();
    }
}