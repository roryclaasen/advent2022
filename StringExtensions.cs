namespace Advent2022.Shared.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class StringExtensions
    {
        /// <summary>
        /// Splits a string into n number of parts.
        /// <see href="https://stackoverflow.com/a/4133475/4498839"/>
        /// </summary>
        /// <param name="s">The string to split.</param>
        /// <param name="partLength">The length of each part.</param>
        /// <param name="skip">The number of chars to skip before the next part.</param>
        /// <returns>IEnumerable of parts.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<string> SplitInParts(this string s, int partLength, int skip = 0)
        {
            ArgumentNullException.ThrowIfNull(s, nameof(s));
            ArgumentNullException.ThrowIfNull(partLength, nameof(partLength));

            if (partLength <= 0)
            {
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));
            }

            for (var i = 0; i < s.Length; i += partLength + skip)
            {
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
            }
        }
    }
}
