using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RA2Survivors
{
    public static class UtilsExtensions
    {
        /// <summary>
        /// Returns count random elements from the array
        public static T[] TakeRandom<T>(this T[] array, int count)
        {
            return array.OrderBy(x => GD.Randf()).Take(count).ToArray();
        }

        public static List<T> TakeRandom<T>(this List<T> list, int count)
        {
            return list.OrderBy(x => GD.Randf()).Take(count).ToList();
        }
    }
}
