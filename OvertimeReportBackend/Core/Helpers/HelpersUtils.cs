using Nager.Holiday;

namespace Core.Helpers
{
    public class HelpersUtils
    {
        public static bool ValidateHours(string initialHour, string finalHour)
        {
            if (TimeSpan.TryParse(initialHour, out TimeSpan tsInitialHour)
            && TimeSpan.TryParse(finalHour, out TimeSpan tsFinalHour))
            {
                return tsInitialHour <= tsFinalHour;
            }
            else
            {
                throw new ArgumentException("Formato de horas incorrectas, porfavor verificalo");
            }
        }
        public static async Task<string> CalculateHourAsync(string initialHour, string finalHour, DateTime date, string Country)
        {
            var countryNager = Country.ToLower() switch
            {
                "colombia" => "CO",
                "argentina" => "AR",
                _ => "CO",
            };
            using var holidayClient = new HolidayClient();
            var holidays = await holidayClient.GetHolidaysAsync(date.Year, countryNager);
            var standardHour = 8.6666;
            var difference = TimeSpan.Parse(finalHour) - TimeSpan.Parse(initialHour);
            bool isHoliday = holidays.Any(day => day.Date == date.Date);
            bool isSunday = date.DayOfWeek == DayOfWeek.Sunday;
            if (isHoliday || isSunday)
            {
                return $"{difference:hh\\:mm}h";
            }
            var resultInTimeSpan = TimeSpan.FromHours(difference.TotalHours - standardHour);
            return $"{resultInTimeSpan:hh\\:mm}h";
        }
    }
}