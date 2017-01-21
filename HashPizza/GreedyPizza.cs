using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class GreedyPizza : Solution
    {
        Pizza pizza;
        public GreedyPizza(Pizza pizza)
        {
            this.pizza = pizza;
            solve();
        }

        enum Orientation
        {
            Vertical, Horizontal
        }

        float GetSliceRatio(Slice slice)
        {
            return
                ((float)pizza.GetNumIngredients(Ingredient.Mushroom, slice)) /
            pizza.GetNumIngredients(Ingredient.Tomato, slice);
        }

        float RatioScore(Slice slice1, Slice slice2)
        {
            return 1 - Math.Abs(GetSliceRatio(slice1) - GetSliceRatio(slice2));
        }

        Tuple<Slice, Slice> DivideSlice(Slice slice, int index, Orientation orientation) {
            Slice slice1;
            Slice slice2;
            if (orientation == Orientation.Horizontal)
            {
                slice1 = new Slice(slice.Left, slice.Right, slice.Top, index);
                slice2 = new Slice(slice.Left, slice.Right, index, slice.Bottom);
            } else
            {
                slice1 = new Slice(slice.Left, index, slice.Top, slice.Bottom);
                slice2 = new Slice(index, slice.Right, slice.Top, slice.Bottom);
            }
            return new Tuple<Slice, Slice>(slice1, slice2);
        }

        void solve()
        {
            Queue<Slice> slicesQueue = new Queue<Slice>();
            slicesQueue.Enqueue(new Slice(0,pizza.NumCols, 0, pizza.NumRows));

            while(slicesQueue.Count > 0)
            {
                Slice slice = slicesQueue.Dequeue();
                if (slice.Size <= pizza.MaxCells)
                {
                    if (pizza.SliceScore(slice) > 0)
                    {
                        Slices.Add(slice);
                    }
                } else {
                    var ori = slice.Height > slice.Width ? Orientation.Horizontal : Orientation.Vertical;
                    var min = ori == Orientation.Vertical ? slice.Left : slice.Top;
                    var max = ori == Orientation.Vertical ? slice.Right : slice.Bottom;

                    float bestScore = 0f;
                    int bestIndex = min;
                    for(int i = min; i < max; i++)
                    {
                        var dividedSlice = DivideSlice(slice, i, ori);

                        var score = RatioScore(dividedSlice.Item1, dividedSlice.Item2);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestIndex = i;
                        }
                    }
                    var bestDivision = DivideSlice(slice, bestIndex, ori);
                    slicesQueue.Enqueue(bestDivision.Item1);
                    slicesQueue.Enqueue(bestDivision.Item2);
                }
            }
        }
    }
}
