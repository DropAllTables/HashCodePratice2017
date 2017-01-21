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

                bool advance = true;

                // Try to break horizontally
                for (int cutoff = 1; cutoff < width - 1; ++cutoff)
                {
                    var subSlice1 = new Slice(slice.Left, slice.Left + cutoff, slice.Top, slice.Bottom);
                    var subSlice2 = new Slice(slice.Left + cutoff, slice.Right, slice.Top, slice.Bottom);

                    int extraY1 = 1;
                    while (slice.Bottom + extraY1 < solution.pizza.NumRows &&
                        subSlice1.Size + subSlice1.Width * extraY1 < solution.pizza.MaxCells &&
                        solution.ExtraSliceFits(new Slice(slice.Left, slice.Left + cutoff, slice.Bottom + 1, slice.Bottom + extraY1)))
                    {
                        ++extraY1;
                    }
                    --extraY1;
                    subSlice1 = new Slice(slice.Left, slice.Left + cutoff, slice.Top, slice.Bottom + extraY1);

                    int extraY2 = 1;
                    while (slice.Bottom + extraY2 < solution.pizza.NumRows &&
                        subSlice2.Size + subSlice2.Width * extraY2 < solution.pizza.MaxCells &&
                        solution.ExtraSliceFits(new Slice(slice.Left + cutoff, slice.Right, slice.Bottom + 1, slice.Bottom + extraY2)))
                    {
                        ++extraY2;
                    }
                    --extraY2;
                    subSlice2 = new Slice(slice.Left + cutoff, slice.Right, slice.Top, slice.Bottom + extraY2);

                    if (solution.pizza.HasMinimumIngredients(subSlice1) && solution.pizza.HasMinimumIngredients(subSlice2))
                    {
                        solution.Slices[i] = subSlice1;
                        solution.Slices.Add(subSlice2);

                        solution.AddSlicePositions(new Slice(slice.Left, slice.Left + cutoff, slice.Bottom + 1, slice.Bottom + extraY1));
                        solution.AddSlicePositions(new Slice(slice.Left + cutoff, slice.Right, slice.Bottom + 1, slice.Bottom + extraY2));

                        advance = false;
                        break;
                    }
                }

                if (!advance)
                {
                    --i;
                    continue;
                }

                // Try to break vertically
                for (int cutoff = 1; cutoff < height - 1; ++cutoff)
                {
                    var subSlice1 = new Slice(slice.Left, slice.Right, slice.Top, slice.Top + cutoff);
                    var subSlice2 = new Slice(slice.Left, slice.Right, slice.Top + cutoff, slice.Bottom);

                    int extraX1 = 1;
                    while (slice.Right + extraX1 < solution.pizza.NumCols &&
                        subSlice1.Size + subSlice1.Height * extraX1 < solution.pizza.MaxCells &&
                        solution.ExtraSliceFits(new Slice(slice.Right + 1, slice.Right + extraX1, slice.Top, slice.Top + cutoff)))
                    {
                        ++extraX1;
                    }
                    --extraX1;
                    subSlice1 = new Slice(slice.Left, slice.Right + extraX1, slice.Top, slice.Top + cutoff);

                    int extraX2 = 1;
                    while (slice.Right + extraX2 < solution.pizza.NumCols &&
                        subSlice2.Size + subSlice2.Height * extraX2 < solution.pizza.MaxCells &&
                        solution.ExtraSliceFits(new Slice(slice.Right + 1, slice.Right + extraX2, slice.Top + cutoff, slice.Bottom)))
                    {
                        ++extraX2;
                    }
                    --extraX2;
                    subSlice2 = new Slice(slice.Left, slice.Right + extraX2, slice.Top + cutoff, slice.Bottom);

                    if (solution.pizza.HasMinimumIngredients(subSlice1) && solution.pizza.HasMinimumIngredients(subSlice2))
                    {
                        solution.Slices[i] = subSlice1;
                        solution.Slices.Add(subSlice2);

                        solution.AddSlicePositions(new Slice(slice.Right + 1, slice.Right + extraX1, slice.Top, slice.Top + cutoff));
                        solution.AddSlicePositions(new Slice(slice.Right + 1, slice.Right + extraX2, slice.Top + cutoff, slice.Bottom));

                        advance = false;
                        break;
                    }
                }

                if (!advance)
                {
                    --i;
                }
            }
        }
    }
}
