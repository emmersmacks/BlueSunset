using System;
using System.Collections.Generic;

namespace CutTwice.Core
{
    public static class RandomExtensions
    {
        public static IList<T> Randomize<T>(this IList<T> list, Random random)
        {
            var result = new List<T>(list);

            for (int i = result.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(i + 1);

                (result[i], result[randomIndex]) =
                    (result[randomIndex], result[i]);
            }

            return result;
        }
    }
}