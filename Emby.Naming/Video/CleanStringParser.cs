using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using MediaBrowser.Model.Configuration;

namespace Emby.Naming.Video
{
    /// <summary>
    /// <see href="http://kodi.wiki/view/Advancedsettings.xml#video" />.
    /// </summary>
    public static class CleanStringParser
    {
        /// <summary>
        /// Attempts to extract clean name with regular expressions.
        /// </summary>
        /// <param name="name">Name of file.</param>
        /// <param name="expressions">List of regex to parse name and year from.</param>
        /// <param name="substitutions">List of substitutions to alter name.</param>
        /// <param name="newName">Parsing result string.</param>
        /// <returns>True if parsing was successful.</returns>
        public static bool TryClean([NotNullWhen(true)] string? name, IReadOnlyList<Regex> expressions, IReadOnlyList<StringSubstitution> substitutions, out string newName)
        {
            if (string.IsNullOrEmpty(name))
            {
                newName = string.Empty;
                return false;
            }

            // Iteratively apply the regexps to clean the string.
            bool cleaned = false;
            for (int i = 0; i < expressions.Count; i++)
            {
                if (TryClean(name, expressions[i], out newName))
                {
                    cleaned = true;
                    name = newName;
                }
            }

            // Iteratively apply the substitutions to clean the string.
            for (int i = 0; i < substitutions.Count; i++)
            {
                if (StringSubstitution.Clean(substitutions[i], name, out newName))
                {
                    cleaned = true;
                    name = newName;
                }
            }

            newName = cleaned ? name : string.Empty;
            return cleaned;
        }

        private static bool TryClean(string name, Regex expression, out string newName)
        {
            var match = expression.Match(name);
            if (match.Success && match.Groups.TryGetValue("cleaned", out var cleaned))
            {
                newName = cleaned.Value;
                return true;
            }

            newName = string.Empty;
            return false;
        }
    }
}
