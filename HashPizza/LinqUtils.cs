using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public static class LinqUtils
    {
        public static Tuple<T, int> WithMax<T>(this IEnumerable<T> self, Func<T, int> evaluator)
        {
            T solutionSoFar = default(T);
            int maxSoFar = int.MinValue;

            foreach (var value in self)
            {
                int result = evaluator(value);
                if (result > maxSoFar)
                {
                    solutionSoFar = value;
                    maxSoFar = result;
                }
            }

            return Tuple.Create(solutionSoFar, maxSoFar);
        }
    }
}
