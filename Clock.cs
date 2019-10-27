using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TalkingClock
{
    enum MinuteType
    {
        Zero,
        To,
        Past
    }

    public class Clock
    {
        private static readonly Dictionary<int, string> Names = new Dictionary<int, string>()
        {
            {0, "o'clock" },
            {1, "one" },
            {2, "two" },
            {3, "three" },
            {4, "four" },
            {5, "five" },
            {6, "six" },
            {7, "seven" },
            {8, "eight" },
            {9, "nine" },
            {10, "ten" },
            {11, "eleven" },
            {12, "twelve" },
            {13, "thirteen" },
            {14, "fourteen" },
            {15, "fifteen" },
            {16, "sixteen" },
            {17, "seventeen" },
            {18, "eighteen" },
            {19, "nineteen" },
            {20, "twenty" },
            {30, "half" }
        };

        private static readonly Dictionary<MinuteType, string> Prepositions = new Dictionary<MinuteType, string>()
        {
            {MinuteType.Zero, "" },
            {MinuteType.To, "to" },
            {MinuteType.Past, "past" }
        };

        public int Hour { get; set; }
        public int Minute { get; set; }

        private static readonly Regex TimeRegex = new Regex(@"^(?<hour>\d{1,2})\:(?<minute>\d{1,2})$");

        public static Clock Parse(string time)
        {
            var match = TimeRegex.Match(time);
            if (match.Success)
            {
                int hour = int.Parse(match.Groups["hour"].Value);
                int minute = int.Parse(match.Groups["minute"].Value);

                return new Clock(hour, minute);
            }

            throw new FormatException("Wrong time format");
        }

        public Clock()
        {
            var now = DateTime.Now;
            Hour = now.Hour;
            Minute = now.Minute;
        }

        public Clock(int hour, int minute)
        {
            bool isHourCorrect = hour >= MinHour && hour <= MaxHour;
            bool isMinuteCorrect = minute >= MinMinute && minute <= MaxMinute;
            if (isHourCorrect && isMinuteCorrect)
            {
                Hour = hour;
                Minute = minute;
            }
            else
            {
                throw new OverflowException($"Value {(isHourCorrect ? minute : hour)} for {(isHourCorrect ? "hours" : "minutes")} is incorrect");
            }
        }

        private string ToName(int value, bool capitalize = false)
        {
            const int baseNumber = 10;
            bool isSmall = value < 20;
            int high = isSmall ? value : (value / baseNumber) * baseNumber;
            int low = isSmall ? 0 : value % baseNumber;

            string highName = Names[high];
            if (capitalize) highName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(highName);
            return low == 0 ? highName : $"{highName} {Names[low]}";
        }

        public const int MinHour = 0;
        public const int MaxHour = 23;
        public const int MinMinute = 0;
        public const int MaxMinute = 59;

        public override string ToString()
        {
            const int hoursInHalfDay = 12;
            const int minutesIntHalfHour = 30;
            const int minutesInHour = 2 * minutesIntHalfHour;

            MinuteType minuteType = Minute == 0 ? MinuteType.Zero : Minute > minutesIntHalfHour ? MinuteType.To : MinuteType.Past;
            int minute = minuteType == MinuteType.To ? minutesInHour - Minute : Minute;

            int hour = (Hour + (minuteType == MinuteType.To ? 1 : 0)) % hoursInHalfDay;
            if (hour == 0) hour = hoursInHalfDay;

            return minuteType == MinuteType.Zero ? $"{ToName(hour, true)} {ToName(minute)}"
                : $"{ToName(minute, true)} {Prepositions[minuteType]} {ToName(hour)}";
        }
    }
}
