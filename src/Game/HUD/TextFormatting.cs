namespace Scorpia.Game.HUD;

public static class TextFormatting
{
    public static string FormatBalance(this int amount, bool inverse = false)
        {
            return $"{GetColourCode(amount, inverse)}{amount.Format()}</text>";
        }
        
        public static string FormatBalance(this float amount, bool inverse = false)
        {
            return $"{GetColourCode(amount, inverse)}{amount.Format()}</text>";
        }

        public static string FormatValid(this int amount, bool valid)
        {
            if (valid)
            {
                return $"<text color='#{Colors.Green}'>{amount}</text>";
            }
            
            return $"<text color='#{Colors.Red}'>{amount}</text>";
        }
        
        public static string FormatValid(this string text, bool valid)
        {
            if (valid)
            {
                return $"<text color='#{Colors.Green}'>{text}</text>";
            }
            
            return $"<text color='#{Colors.Red}'>{text}</text>";
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
            var rounded = (int)Math.Round(num);

            return Format(rounded);
        }
        
        private static string GetColourCode(float amount, bool inverse)
        {
            var colour = amount switch
            {
                > 0 => inverse ? $"<text color='#{Colors.Red}'>" : $"<text color=#{Colors.Green}>",
                < 0 => inverse ? $"<text color='#{Colors.Green}'>" : $"<text color='#{Colors.Red}'>",
                _ => "<text>"
            };

            return colour;
        }
}