namespace Main.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhiteSpaces(this string word)
        {
            return word.Replace(" ", "");
        }
    }
}
