using System.Globalization;
using System.Reflection;

namespace BlazorCalendar
{
    public static class DateTimeUtil
    {
        private static readonly PersianCalendar _pc = new();
        public static System.DateTime TodayDate
        {
            get
            {

                return DateTime.Today;
                //int year = _pc.GetYear(System.DateTime.Now);
                //int month = _pc.GetMonth(System.DateTime.Now);
                //int day = _pc.GetDayOfMonth(DateTime.);
                //System.DateTime today = _pc.ToDateTime(year, month, day, 0, 0, 0, 0);
                //return today;

            }
        }

        public static CultureInfo GetPersianCulture()
        {
            var culture = new CultureInfo("fa-IR");
            DateTimeFormatInfo formatInfo = culture.DateTimeFormat;
            formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
            formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
            var monthNames = new[]
            {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
            "اسفند",
            "",
        };
            formatInfo.AbbreviatedMonthNames =
                formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
            formatInfo.AMDesignator = "ق.ظ";
            formatInfo.PMDesignator = "ب.ظ";
            formatInfo.ShortDatePattern = "yyyy/MM/dd";
            formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
            formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
            System.Globalization.Calendar cal = new PersianCalendar();
            FieldInfo fieldInfo = culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
                fieldInfo.SetValue(culture, cal);
            FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (info != null)
                info.SetValue(formatInfo, cal);
            culture.NumberFormat.NumberDecimalSeparator = "/";
            culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
            culture.NumberFormat.NumberNegativePattern = 0;
            return culture;
        }

        public static int GetPersianDayOfMonth(this DateTime dateTime)
        {
            return _pc.GetDayOfMonth(dateTime);
        }


        public static int GetPersianMonth(this DateTime dateTime)
        {
            return _pc.GetMonth(dateTime);
        }

    }


}
