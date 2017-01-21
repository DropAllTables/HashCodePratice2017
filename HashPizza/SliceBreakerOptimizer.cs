using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class SliceBreakerOptimizer
    {
        public static void Optimize(Solution solution)
        {
            for (int i = 0; i < solution.Slices.Count; ++i)
            {
                var slice = solution.Slices[i];

                int width = slice.Width;
                int height = slice.Height;

                // Try to break horizontally
                for (int cutoff = 1; cutoff < width - 1; ++cutoff)
                {
                    var subSlice1 = new Slice(slice.Left, slice.Left + cutoff, slice.Top, slice.Bottom);
                    var subSlice2 = new Slice(slice.Left + cutoff, slice.Right, slice.Top, slice.Bottom);

                    if (solution.pizza.HasMinimumIngredients(subSlice1) && solution.pizza.HasMinimumIngredients(subSlice2))
                    {
                        solution.Slices[i] = subSlice1;
                        solution.Slices.Add(subSlice2);
                        --i;
                        break;
                    }
                }

                // Try to break vertically
                for (int cutoff = 1; cutoff < height - 1; ++cutoff)
                {
                    var subSlice1 = new Slice(slice.Left, slice.Right, slice.Top, slice.Top + cutoff);
                    var subSlice2 = new Slice(slice.Left, slice.Right, slice.Top + cutoff, slice.Bottom);

                    if (solution.pizza.HasMinimumIngredients(subSlice1) && solution.pizza.HasMinimumIngredients(subSlice2))
                    {
                        solution.Slices[i] = subSlice1;
                        solution.Slices.Add(subSlice2);
                        --i;
                        break;
                    }
                }
            }
        }
    }
}
