using System;
using UI;

namespace Utils
{
    public static class StringExtensions
    {
        public static string FormatBalance(this int amount)
        {
            return $"{GetColourCode(amount)}{amount.Format()}</color>";
        }
        
        public static string FormatBalance(this float amount)
        {
            return $"{GetColourCode(amount)}{amount.Format()}</color>";
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
            if (rounded >= 100000000) {
                return (rounded / 1000000D).ToString("0.#M");
            }
            if (rounded >= 1000000) {
                return (rounded / 1000000D).ToString("0.##M");
            }
            if (rounded >= 100000) {
                return (rounded / 1000D).ToString("0.#k");
            }
            if (rounded >= 10000) {
                return (rounded / 1000D).ToString("0.##k");
            }

            return rounded.ToString();
        }
        
        private static string GetColourCode(float amount)
        {
            var colour = amount switch
            {
                > 0 => $"<color=#{Colours.Green}>",
                < 0 => $"<color=#{Colours.Red}>",
                _ => ""
            };

            return colour;
        }
    }
}