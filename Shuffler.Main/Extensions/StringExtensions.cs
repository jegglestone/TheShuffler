namespace Main.Extensions
{
    using System.Text.RegularExpressions;
    using Constants;

    public static class StringExtensions
    {
        public static string RemoveWhiteSpaces(this string word)
        {
            return word.Replace(" ", "");
        }

        public static int CountTimesThisStringAppearsInThatString(this string orig, string find)
        {
            return Regex.Matches(Regex.Escape(orig), find).Count;
        }

        public static bool IsBreakerPunctuation(this string str)
        {
            return str == TagMarks.BreakerPunctuation;
        }
    }
}
