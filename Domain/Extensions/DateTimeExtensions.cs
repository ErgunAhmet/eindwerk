using System.Globalization;
using System.Text.Json;

namespace Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToUserString(this DateTime dateTime, bool displayTime = false, bool forceBelgianTime = false, bool includeSeconds = false)
        {
            if (forceBelgianTime)
                dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));

            var dateText = dateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (displayTime)
            {
                dateText += dateTime.ToString(" HH:mm", CultureInfo.InvariantCulture);

                if (includeSeconds)
                    dateText += dateTime.ToString(" s", CultureInfo.InvariantCulture) + "s";
            }

            if (dateTime.Kind == DateTimeKind.Utc)
                dateText += " UTC";

            return dateText;
        }

        public static string ToJsonString(this DateTime dateTime)
        {
            var json = JsonSerializer.Serialize(dateTime);
            return json.Replace("\"", string.Empty);
        }

        public static bool TryFromJsonString(this string dateTimeJson, out DateTime dateTime)
        {
            dateTime = default;

            if (string.IsNullOrEmpty(dateTimeJson))
                return false;

            if (!dateTimeJson.StartsWith("\""))
                dateTimeJson = $"\"{dateTimeJson}\"";

            try
            {
                dateTime = JsonSerializer.Deserialize<DateTime>(dateTimeJson);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string TimeSinceLastUpdate(this DateTime dateTime)
        {
            var timeDiff = DateTime.UtcNow - dateTime;
            if (timeDiff.Days > 0)
            {
                return $"{timeDiff.Days} days";
            }
            if (timeDiff.Hours > 0)
            {
                return $"{timeDiff.Hours} hours";
            }
            if (timeDiff.Minutes > 0)
            {
                return $"{timeDiff.Minutes} minutes";
            }
            return $"{timeDiff.Seconds} seconds";
        }

        public static double ToEpoch(this DateTime dateTime)
        {
            TimeSpan t = dateTime - new DateTime(1970, 1, 1);
            return t.TotalSeconds;
        }

        public static string ShortFormat(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalHours < 0)
                return $"{Math.Ceiling(timeSpan.TotalHours)}:{Math.Abs(timeSpan.Minutes):00}";
            return $"{Math.Floor(timeSpan.TotalHours)}:{Math.Abs(timeSpan.Minutes):00}";
        }
    }
}
