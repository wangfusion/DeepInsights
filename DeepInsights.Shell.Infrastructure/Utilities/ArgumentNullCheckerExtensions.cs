using System;
using System.Collections.Generic;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    public static class ArgumentNullCheckerExtensions
    {
        public static void ThrowIfNull<T>(this T value, string argument)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                throw new ArgumentNullException(argument);
            }
        }
    }
}
