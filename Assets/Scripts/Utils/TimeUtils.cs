using UnityEngine;

namespace Utils
{
    public static class TimeUtils
    {
        public static string FormatTimer(float seconds)
        {
            var m = Mathf.FloorToInt(seconds / 60);
            var s = Mathf.RoundToInt(seconds - m * 60);

            return $"{FormatNumberToTwoDigits(m)}:{FormatNumberToTwoDigits(s)}";
        }

        public static string FormatNumberToTwoDigits(int number)
        {
            return number.ToString("D2");
        }
    }
}