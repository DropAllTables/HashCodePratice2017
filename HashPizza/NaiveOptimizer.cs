using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class NaiveOptimizer
    {
        public static void Optimize(Solution solution)
        {
            for (int y = 0; y < solution.pizza.NumRows; ++y)
            {
                for (int x = 0; x < solution.pizza.NumCols; ++x)
                {
                    if (!solution.UsesPosition(y, x))
                    {
                        TryAddSliceAt(solution, x, y);
                    }
                }
            }
        }

        private static void TryAddSliceAt(Solution solution, int x, int y)
        {
            int minSize = solution.pizza.MinIngredients * 2;

            int maxRows = 1;
            for (; maxRows < solution.pizza.MaxCells && y + maxRows < solution.pizza.NumRows && !solution.UsesPosition(y + maxRows, x); ++maxRows)
            {
            }
            int maxCols = 1;
            for (; maxCols < solution.pizza.MaxCells && x + maxCols < solution.pizza.NumCols && !solution.UsesPosition(y, x + maxCols); ++maxCols)
            {
            }

            for (int size = Math.Max(maxRows, maxCols); size >= minSize; --size) {
                for (int width = size - 1; width > 0; --width)
                {
                    int height = size - width;
                    if (width > maxCols)
                    {
                        break;
                    }
                    if (height > maxRows)
                    {
                        continue;
                    }

                    var slice = new Slice(x, x + width - 1, y, y + height - 1);
                    if (slice.Right >= solution.pizza.NumCols || slice.Bottom >= solution.pizza.NumRows || slice.Size > solution.pizza.MaxCells)
                    {
                        continue;
                    }

                    if (solution.pizza.HasMinimumIngredients(slice) && solution.ExtraSliceFits(slice))
                    {
                        solution.AddSlice(slice);
                        return;
                    }
                }
            }
        }
    }
}
