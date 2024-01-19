using Iso639;
using System;
using System.Globalization;
using System.Linq;

namespace OPMedia.Core.Utilities
{
    public static class LanguageHelper
    {
        public static string DisplayName(this Language language)
        {
            var name = language?.Culture?.EnglishName;
            if (name?.Length > 0)
            {
                int idx = name.IndexOf('(');
                if (idx > 0)
                    name = name.Substring(0, idx);
            }

            return name;
        }

        public static Language Lookup(string lang)
        {
            if (lang?.Length >= 2)
            {
                bool lookupByCulture = false;

            lookup:
                var language = Language.Database.Where(l =>
                (
                    string.Equals(l.Part1, lang, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(l.Part2, lang, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(l.Part2B, lang, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(l.Part3, lang, StringComparison.OrdinalIgnoreCase) ||

                    string.Equals(l.Culture.TwoLetterISOLanguageName, lang, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(l.Culture.ThreeLetterISOLanguageName, lang, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(l.Culture.ThreeLetterWindowsLanguageName, lang, StringComparison.OrdinalIgnoreCase)

                )).FirstOrDefault();

                if (language != null || lookupByCulture)
                    return language;

                try
                {
                    var ci = new CultureInfo(lang);
                    if (ci != null)
                    {
                        lang = ci.TwoLetterISOLanguageName;
                        lookupByCulture = true;
                        goto lookup;
                    }
                }
                catch
                { }
            }

            return null;
        }

        public static bool IsSameLanguage(string lang1, string lang2)
        {
            var l1 = Lookup(lang1)?.Culture?.LCID;
            var l2 = Lookup(lang2)?.Culture?.LCID;
            return l1.HasValue && l2.HasValue && l1.Value == l2.Value;
        }

        public static bool IsSameLanguage(int lcid, string lang)
        {
            var l1 = Lookup(lang)?.Culture?.LCID;
            return l1.HasValue && l1.Value == lcid;
        }
    }
}
