using System;
using UI;

namespace Utils
{
    public static class StringExtensions
    {
        public static string FormatBalance(this int amount, bool inverse = false)
        {
            return $"{GetColourCode(amount, inverse)}{amount.Format()}</color>";
        }
        
        public static string FormatBalance(this float amount, bool inverse = false)
        {
            return $"{GetColourCode(amount, inverse)}{amount.Format()}</color>";
        }

        public static string FormatValid(this int amount, bool valid)
        {
            if (valid)
            {
                return $"<color=#{Colours.Green}>{amount}</color>";
            }
            
            return $"<color=#{Colours.Red}>{amount}</color>";
        }
        
        public static string FormatValid(this string text, bool valid)
        {
            if (valid)
            {
                return $"<color=#{Colours.Green}>{text}</color>";
            }
            
            return $"<color=#{Colours.Red}>{text}</color>";
        }

        public static string Format(this int num)
        {
            if (num >= 100000000) {
                return (num / 1000000D).ToString("0.#M");
            }
            if (num >= 1000000) {
                return (num / 1000000D).ToString("0.##M");
            }
            if (num >= 100000) {
                return (num / 1000D).ToString("0.#k");
            }
            if (num >= 10000) {
                return (num / 1000D).ToString("0.##k");
            }

            return num.ToString();
        }
        
        public static string Format(this float num)
        {
            var rounded = (int)Math.Floor(num);
            
            return Format(rounded);
        }
        
        private static string GetColourCode(float amount, bool inverse)
        {
            var colour = amount switch
            {
                > 0 => inverse ? $"<color=#{Colours.Red}>" : $"<color=#{Colours.Green}>",
                < 0 => inverse ? $"<color=#{Colours.Green}>" : $"<color=#{Colours.Red}>",
                _ => ""
            };

            return colour;
        }
    }
}