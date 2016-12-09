using System;

namespace Main.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        public static void ShiftElement<T>(this T[] array, int oldIndex, int newIndex)
        {
            // TODO: Argument validation
            if (oldIndex == newIndex)
            {
                return; // No-op
            }
            T tmp = array[oldIndex];
            if (newIndex < oldIndex)
            {
                // Need to move part of the array "up" to make room
                Array.Copy(array, newIndex, array, newIndex + 1, oldIndex - newIndex);
            }
            else
            {
                // Need to move part of the array "down" to fill the gap
                Array.Copy(array, oldIndex + 1, array, oldIndex, newIndex - oldIndex);
            }
            array[newIndex] = tmp;
        }
    }


}