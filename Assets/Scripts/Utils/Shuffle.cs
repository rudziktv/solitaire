using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Utils
{
    public class Shuffle
    {
        public static void StandardShuffle<T>(T[] a)
        {
            var rng = new Random();
            var n = a.Length;

            for (var i = n - 1; i > 0; i--)
            {
                var j = rng.Next(i + 1);
                (a[i], a[j]) = (a[j], a[i]);
            }
        }
    }
}