namespace Main.Helper
{
    using System.Linq;

    public static class ArrayUtility
    {
        public static void SplitArrayAtPosition<T>(T[] array, int index, out T[] first, out T[] second)
        {
            first = array.Take(index).ToArray();
            second = array.Skip(index).ToArray();
        }
    }
}