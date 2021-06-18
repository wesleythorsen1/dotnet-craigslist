using System;

namespace Craigslist
{
    public static class Utility
    {
        public static T? ConvertOrDefault<T>(string value) where T : IConvertible
        {
            if (value == default)
            {
                return default;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}