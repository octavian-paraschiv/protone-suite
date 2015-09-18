using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace OPMedia.Core.Utilities
{
    public enum WordCasing
    {
        KeepCase,
        SentenceCase,
        CapitalizeWords
    }

    public static class StringUtils
    {
        private static string __DateTimeFormatString;

        public static string ToValidXml(this StringBuilder sb)
        {
            return StripInvalidXmlChars(sb.ToString());
        }

        public static string StripInvalidXmlChars(string text)
        {
            // From xml spec valid chars: 
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]     
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
            string re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-u10FFFF]";
            return Regex.Replace(text, re, "");
        }

        public static string StripInvalidPathChars(string newName)
        {
            string retVal = newName.Replace("<", string.Empty).Replace(">", string.Empty).Replace("�", "ö");
            foreach (char cInvalid in Path.GetInvalidFileNameChars())
            {
                retVal = retVal.Replace(cInvalid, '_');
            }

            return retVal;
        }

        public static string Limit(string input, int maxLen)
        {
            if (input != null && maxLen > 3 && input.Length > maxLen)
            {
                input = input.Substring(0, maxLen - 3) + "...";
            }

            return input;
        }

        public static string TakeValid(string master, string slave)
        {
            if (string.IsNullOrEmpty(master))
            {
                if (!string.IsNullOrEmpty(slave))
                    return slave;
            }

            return master;
        }

        public static void RebuildDateTimeFormatString()
        {
            DateTimeFormatInfo dtfi = CultureInfo.CurrentUICulture.DateTimeFormat;

            string[] dateFmtParts = dtfi.ShortDatePattern.ToLowerInvariant().Split((dtfi.DateSeparator + " ").ToCharArray());
            string[] timeFmtParts = dtfi.ShortTimePattern.ToLowerInvariant().Split((dtfi.TimeSeparator + " ").ToCharArray());

            string fmt = string.Empty;

            foreach (string part in dateFmtParts)
            {
                if (part.StartsWith("y"))
                {
                    fmt += "yyyy";
                    fmt += dtfi.DateSeparator;
                }
                else if (part.StartsWith("m"))
                {
                    fmt += "MM";
                    fmt += dtfi.DateSeparator;
                }
                else if (part.StartsWith("d"))
                {
                    fmt += "dd";
                    fmt += dtfi.DateSeparator;
                }
            }

            fmt = fmt.TrimEnd(dtfi.DateSeparator.ToCharArray());
            fmt += " ";

            bool hasSeconds = dtfi.ShortTimePattern.Contains("ss");
            bool hasAmPm = dtfi.ShortTimePattern.Contains("tt");

            foreach (string part in timeFmtParts)
            {
                if (part.StartsWith("h"))
                {
                    fmt += hasAmPm ? "hh" : "HH";
                    fmt += dtfi.TimeSeparator;
                }
                else if (part.StartsWith("m"))
                {
                    fmt += hasSeconds ? "mm" : "mm" + dtfi.TimeSeparator + "ss";
                    fmt += dtfi.TimeSeparator;
                }
                else if (part.StartsWith("s"))
                {
                    fmt += "ss";
                    fmt += dtfi.TimeSeparator;
                }
                else if (part.StartsWith("t"))
                {
                    fmt += "tt";
                    fmt += dtfi.TimeSeparator;
                }
            }

            __DateTimeFormatString = fmt.TrimEnd(dtfi.TimeSeparator.ToCharArray()).Replace(":tt", " tt");
        }

        public static string BuildTimeString(DateTime dt)
        {
            return dt.ToString(__DateTimeFormatString);
        }

        public static string GenerateRandomToken(int len)
        {
            string token = string.Empty;

            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < len; i++)
            {
                token += (char)rnd.Next((int)'A', (int)'Z');
            }

            return token;
        }

        public static string EncodeMessage(string input)
        {
            StringBuilder retVal = new StringBuilder();
            foreach (char c in input.ToCharArray())
            {
                retVal.Append((char)(0xffff ^ c)); // encoding is reversible
            }
            return retVal.ToString();
        }

        public static void ReplaceToken(ref string target, string token, string source)
        {
            while (true)
            {
                int i1 = target.IndexOf(token);
                if (i1 < 0) break;

                int i2 = target.IndexOf(">", i1 + token.Length);
                if (i2 < 0) break;

                int start = 1, stop = source.Length; // Index 1-based

                string fullToken = target.Substring(i1, i2 - i1 + 1);
                string limits = fullToken.Replace(token, string.Empty).Replace(">", string.Empty);

                string[] tokenLimits = limits.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (tokenLimits.Length > 0) int.TryParse(tokenLimits[0], out start);
                if (tokenLimits.Length > 1) int.TryParse(tokenLimits[1], out stop);

                if ((stop < 1 || start >= stop) && (start != 0 && stop != 0))
                {
                    target = string.Empty;
                    break;
                }

                if (start < 1) start = 1;
                if (stop > source.Length) stop = source.Length;

                string replacement = source.Substring(start - 1, stop - start + 1);
                target = target.Replace(fullToken, replacement);
            }
        }

        public static Dictionary<string, string> Tokenize(string source, string pattern)
        {
            string target = source.Clone() as string;
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            List<string> tokens = new List<string>();
            List<string> seps = new List<string>();

            string sep = string.Empty;
            string tok = string.Empty;
            
            bool inToken = false;
            bool inSep = false;

            // Identify tokens and separators
            foreach(char c in pattern)
            {
                if (c == '<')
                {
                    // tok start, sep end
                    inToken = true;
                    inSep = false;

                    if (sep.Length > 0)
                    {
                        seps.Add(sep);
                        sep = string.Empty;
                    }
                }
                else if (c == '>')
                {
                    // tok end, sep start
                    tok += c;

                    inToken = false;
                    inSep = true;

                    if (tok.Length > 0)
                    {
                        tokens.Add(tok);
                        tok = string.Empty;
                    }
                }

                if (inToken)
                {
                    tok += c;
                }
                else if (inSep && c != '>')
                {
                    sep += c;
                }
            }

            // Replace separators in source string
            foreach (string separator in seps)
            {
                target = ReplaceFirstOccurence(target, separator, "^");
            }

            // Split source
            string[] srcParts = target.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

            // Fill in tokens vs. parts table
            for (int i = 0; i < Math.Min(srcParts.Length, tokens.Count); i++)
            {
                retVal.Add(tokens[i], srcParts[i]);
            }

            return retVal;
        }

        public static string ReplaceFirstOccurence(string source, string oldVal, string newVal)
        {
            string target = source.Clone() as string;

            int i1 = target.IndexOf(oldVal);
            if (i1 >= 0 && i1 + oldVal.Length < source.Length)
            {
                string s1 = target.Substring(0, i1 + oldVal.Length).Replace(oldVal, "^");

                string s2 = string.Empty;
                if (i1 + oldVal.Length < source.Length)
                {
                    s2 = target.Substring(i1 + oldVal.Length);
                }
                
                target = s1 + s2;
            }

            return target;
        }

        public static string Capitalize(string target, WordCasing casing)
        {
            if (!string.IsNullOrEmpty(target))
            {
                switch (casing)
                {
                    case WordCasing.SentenceCase:
                        return MakeSentenceCase(target);

                    case WordCasing.CapitalizeWords:
                        return CapitalizeWords(target);
                }
            }

            return target;
        }

        public static string CapitalizeWords(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string retVal = string.Empty;

                string[] parts = str.Split(new char[] { ' ' });
                foreach (string part in parts)
                {
                    retVal += MakeSentenceCase(part) + " ";
                }

                return retVal.Trim();
            }

            return str;
        }

        public static string MakeSentenceCase(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                char[] chars = str.Trim().ToCharArray();
                chars[0] = char.ToUpperInvariant(chars[0]);
                string retVal = new string(chars);
                return retVal.Trim();
            }

            return str;
        }

        public static string FromStringArray(string[] array, char delim)
        {
            string retVal = string.Empty;

            if (array != null && array.Length > 0)
            {
                foreach (string field in array)
                {
                    retVal += field;
                    retVal += delim;
                }
            }

            return retVal.TrimEnd(new char[] { delim });
        }

        public static string[] ToStringArray(string str, char delim)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Split(new char[] { delim }, StringSplitOptions.RemoveEmptyEntries);
            }

            return null;
        }

        public static string GetUniqueFileName(ref DateTime timeStamp)
        {
            timeStamp = DateTime.Now;
            return string.Format("{0:d4}-{1:d2}-{2:d2}@{3:d2}_{4:d2}_{5:d2}.{6:d3}",
                timeStamp.Year, timeStamp.Month, timeStamp.Day,
                timeStamp.Hour, timeStamp.Minute, timeStamp.Second,
                timeStamp.Millisecond);
        }

        public static string ConvertDiacriticalsToRtfTags(string input)
        {
            string stage1 = FixDiacriticals(input);
            string stage2 = stage1.Replace("Ş", "\\'aa").Replace("Ţ", "\\'de").Replace("Ă", "\\'c3").Replace("Î", "\\'ce").Replace("Â", "\\'c2");
            string stage3 = stage2.Replace("ş", "\\'ba").Replace("ţ", "\\'fe").Replace("ă", "\\'e3").Replace("î", "\\'ee").Replace("â", "\\'e2");
            return stage3;
        }

        public static string FixDiacriticals(string p)
        {
            return p.Replace("º", "ş").Replace("þ", "ţ").Replace("ª", "Ş").Replace("Þ", "Ţ");
        }

        public static string MakeFilterString(string s, bool dereferenceLinks)
        {
            if ((s == null) || (s.Length == 0))
            {
                if (dereferenceLinks && (Environment.OSVersion.Version.Major >= 5))
                {
                    s = " |*.*";
                }
                else if (s == null)
                {
                    return null;
                }
            }
            int length = s.Length;
            char[] destination = new char[length + 2];
            s.CopyTo(0, destination, 0, length);
            for (int i = 0; i < length; i++)
            {
                if (destination[i] == '|')
                {
                    destination[i] = '\0';
                }
            }
            destination[length + 1] = '\0';
            return new string(destination);
        }


        public static bool StringMatchesPattern(string str, string pattern)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            if (pattern == "*")
                return true;

            if (pattern == "*.*" && (str.Contains(".")))
                return true;

#if HAVE_VB_COMPILER_SERVICES
            return Microsoft.VisualBasic.CompilerServices.Operators.LikeString(str, 
                pattern, Microsoft.VisualBasic.CompareMethod.Text);
#else
            try
            {
                Regex regex = FindFilesPatternToRegex.Convert(pattern);
                return regex.IsMatch(str);
            }
            catch
            {
                return true;
            }
        }
#endif
    }

#if HAVE_VB_COMPILER_SERVICES
#else
    public static class FindFilesPatternToRegex
    {
        private static Regex HasQuestionMarkRegEx = new Regex(@"\?", RegexOptions.Compiled);
        private static Regex IlegalCharactersRegex = new Regex("[" + @"\/:<>|" + "\"]", RegexOptions.Compiled);
        private static Regex CatchExtentionRegex = new Regex(@"^\s*.+\.([^\.]+)\s*$", RegexOptions.Compiled);
        private static string NonDotCharacters = @"[^.]*";

        public static Regex Convert(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException();
            }
            pattern = pattern.Trim();
            if (pattern.Length == 0)
            {
                throw new ArgumentException("Pattern is empty.");
            }
            if (IlegalCharactersRegex.IsMatch(pattern))
            {
                throw new ArgumentException("Patterns contains ilegal characters.");
            }
            bool hasExtension = CatchExtentionRegex.IsMatch(pattern);
            bool matchExact = false;
            if (HasQuestionMarkRegEx.IsMatch(pattern))
            {
                matchExact = true;
            }
            else if (hasExtension)
            {
                matchExact = CatchExtentionRegex.Match(pattern).Groups[1].Length != 3;
            }
            string regexString = Regex.Escape(pattern);
            regexString = "^" + Regex.Replace(regexString, @"\\\*", ".*");
            regexString = Regex.Replace(regexString, @"\\\?", ".");
            if (!matchExact && hasExtension)
            {
                regexString += NonDotCharacters;
            }
            regexString += "$";
            Regex regex = new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex;
        }
    }
#endif
}
