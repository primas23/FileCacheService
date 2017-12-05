using System;

namespace FCS.Contracts
{
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets the date time now.
        /// </summary>
        /// <returns>Returns either the provide datetime property or the current datetime now.</returns>
        DateTime GetDateTimeNow();
    }
}
