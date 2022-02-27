using System;

namespace Scorpia.Assets.Scripts
{
    public class ScorpiaDate
    {
        private readonly int ticks;

        private const int START_YEAR = 3000;

        private readonly string[] monthNames = { "Tishri", "Marẖeshvan", "Kislev", "Tevet", "Shvat", "Adar", "Nisan", "Iyyar", "Sivan", "Tammuz", "Av", "Elul" };

        public ScorpiaDate(int ticks = 0)
        {
            this.ticks = ticks;
        }

        public int Day
        {
            get
            {
                return ticks % 30 + 1;
            }
        }

        public int Month
        {
            get
            {
                return (int)Math.Floor(ticks / 30.0) % 12 + 1;
            }
        }

        public int Year
        {
            get
            {
                return START_YEAR + (int)Math.Floor(ticks / 360.0);
            }
        }

        public string ToString(string format)
        {
            if (format == "D")
            {
                return $"{Day} {monthNames[Month - 1]} {Year}";
            }
            else if (format == "m")
            {
                return $"{Day}. {Month}";
            }
            else if (format == "M")
            {
                return $"{Day} {monthNames[Month - 1]}";
            }
            
            return $"{Day}-{Month}-{Year}";
        }
    }
}