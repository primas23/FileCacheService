using System;

using FCS.Contracts;

namespace FCS.Utils
{
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Gets the date time now.
        /// </summary>
        /// <returns>Returns either the provide datetime property or the current datetime now.</returns>
        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
    }
}
