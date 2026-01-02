#nullable disable
#pragma warning disable CS1591
// , CA1819
using System;
using System.Globalization;
using System.IO;

namespace MediaBrowser.Model.Configuration
{
    /// <summary>
    /// Regular expressions for parsing TV Episodes.
    /// </summary>
    public class StringSubstitution
    {
        public StringSubstitution()
        {
        }

        public string Match { get; set; }

        public string Substitute { get; set; }

        /// <summary>
        /// Performs substitution on string where match exists.
        /// </summary>
        /// <param name="substitution">List of substitutions to alter name.</param>
        /// <param name="value">Name of file.</param>
        /// <param name="newValue">Subtituting result string.</param>
        /// <returns>True if parsing was successful.</returns>
        public static bool Clean(StringSubstitution substitution, string value, out string newValue)
        {
            bool changed = false;
            int offset = 0;
            string original = value;

            while ((offset = value.IndexOf(substitution.Match, offset, StringComparison.CurrentCulture)) != -1)
            {
                value = string.Concat(value.AsSpan(0, offset), substitution.Substitute, value.AsSpan(offset + substitution.Match.Length));
                offset += substitution.Substitute.Length;
                changed = true;
            }

            newValue = changed ? value : string.Empty;
            return changed;
        }
    }
}
