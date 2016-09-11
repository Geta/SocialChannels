using System;
using EPiServer.Framework.Localization;

namespace Geta.SocialChannels
{
    public static class DateTimeExtensions
    {
        const double ApproxDaysPerMonth = 30.4375;

        const double ApproxDaysPerYear = 365.25;

        public static string ToTimeSinceString(this DateTime dateTime)
        {
            string sentTime;
            var timeSpan = DateTime.Now - dateTime;
            var iDays = timeSpan.Days;

            //Calculate years as an integer division
            var iYear = (int)(iDays / ApproxDaysPerYear);

            // Decrease remaing days
            iDays -= (int)(iYear * ApproxDaysPerYear);

            // Calculate months as an integer division
            var iMonths = (int)(iDays / ApproxDaysPerMonth);

            // Decrease remaing days
            iDays -= (int)(iMonths * ApproxDaysPerMonth);

            if (iYear > 0)
            {
                sentTime = iYear == 1
                    ? LocalizationService.Current.GetString("/views/format/datetime/ayearago")
                    : string.Format(LocalizationService.Current.GetString("/views/format/datetime/yearsago"), iYear);
            }
            else
            {
                if (iMonths > 0)
                {
                    sentTime = iMonths == 1
                        ? LocalizationService.Current.GetString("/views/format/datetime/amonthago")
                        : string.Format(LocalizationService.Current.GetString("/views/format/datetime/monthsago"), iMonths);
                }
                else
                {
                    if (iDays > 7)
                    {
                        var iWeeks = iDays / 7;

                        sentTime = iWeeks == 1
                            ? LocalizationService.Current.GetString("/views/format/datetime/aweekago")
                            : string.Format(LocalizationService.Current.GetString("/views/format/datetime/weeksago"), iWeeks);
                    }
                    else
                    {
                        if (iDays > 0)
                        {
                            sentTime = iDays == 1
                                ? LocalizationService.Current.GetString("/views/format/datetime/yesterday")
                                : string.Format(LocalizationService.Current.GetString("/views/format/datetime/daysago"), iDays);
                        }
                        else
                        {
                            if (timeSpan.Hours < 1)
                            {
                                sentTime = timeSpan.Minutes == 1
                                    ? LocalizationService.Current.GetString("/views/format/datetime/aminuteago")
                                    : string.Format(
                                        LocalizationService.Current.GetString("/views/format/datetime/minutesago"),
                                        timeSpan.Minutes);
                            }
                            else
                            {
                                sentTime = timeSpan.Hours == 1
                                    ? LocalizationService.Current.GetString("/views/format/datetime/ahourago")
                                    : string.Format(
                                        LocalizationService.Current.GetString("/views/format/datetime/hoursago"),
                                        timeSpan.Hours);
                            }
                        }
                    }
                }
            }

            return sentTime;
        }

        public static string ToGenericFormatDateString(this DateTime dateTime)
        {
            const string resourceKey = "/views/format/datetime/generic";
            var formatString = LocalizationService.Current.GetString(resourceKey);

            return dateTime.ToString(!resourceKey.Equals(formatString) ? formatString : "MMMM d, yyyy");
        }


        public static DateTime NormalizeToHours(this DateTime date)
        {
            return date.Date.AddHours((double)date.Hour);
        }

        public static DateTime? NormalizeToHours(this DateTime? nullableDate)
        {
            return nullableDate?.Date.AddHours((double)nullableDate.Value.Hour);
        }

        public static string ToTimeDurationString(this TimeSpan timeSpan)
        {
            if (timeSpan.Minutes >= 60)
            {
                return timeSpan.ToString(@"hh\:mm\:ss");
            }
            return timeSpan.ToString(@"mm\:ss");
        }
    }
}