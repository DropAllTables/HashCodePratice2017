using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class GreedyPizza : Solution
    {
        private GreedyPizza(Pizza pizza) : base(pizza)
        {
            //solve();
            FillEmptySpaces();
        }

        enum Orientation
        {
            Vertical, Horizontal
        }

        float GetSliceRatio(Slice slice)
        {
            var numMushrooms = pizza.GetNumIngredients(Ingredient.Mushroom, slice);
            var numTomatoes = slice.Size - numMushrooms;
            return ((float)numMushrooms) / numTomatoes;
        }

        public static Solution Solve(Pizza pizza)
        {
            var baseSolution = new GreedyPizza(pizza);

            var newSolution = new Solution(pizza);
            foreach (var slice in baseSolution.Slices)
            {
                newSolution.AddSlice(slice);
            }

            return newSolution;
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
                slice2 = new Slice(slice.Left, slice.Right, index + 1, slice.Bottom);
            } else
            {
                slice1 = new Slice(slice.Left, index, slice.Top, slice.Bottom);
                slice2 = new Slice(index + 1, slice.Right, slice.Top, slice.Bottom);
            }
            return new Tuple<Slice, Slice>(slice1, slice2);
        }

        void solve()
        {
            var rand = new Random();
            Queue<Slice> slicesQueue = new Queue<Slice>();
            slicesQueue.Enqueue(new Slice(0,pizza.NumCols - 1, 0, pizza.NumRows - 1));

            while(slicesQueue.Count > 0)
            {
                Slice slice = slicesQueue.Dequeue();
                if (slice.Size <= pizza.MaxCells)
                {
                    if (pizza.HasMinimumIngredients(slice))
                    {
                        AddSlice(slice);
                    }
                } else {
                    var ori = slice.Height > slice.Width ? Orientation.Horizontal : Orientation.Vertical;
                    var min = ori == Orientation.Vertical ? slice.Left : slice.Top;
                    var max = ori == Orientation.Vertical ? slice.Right : slice.Bottom;

                    float bestScore = 0f;
                    int bestIndex = min;
                    object locker = new object();
                    Parallel.For(min + 1, max - 1, i => {
                        var dividedSlice = DivideSlice(slice, i, ori);

                        var score = RatioScore(dividedSlice.Item1, dividedSlice.Item2);
                        if (score > bestScore)
                        {
                            lock(locker)
                            {
                                bestScore = score;
                                bestIndex = i;
                            }
                        }
                    });
                    var variation = rand.Next(-20, 21);
                    bestIndex = Math.Max(Math.Min(bestIndex + variation, max - 2), min + 1);
                    var bestDivision = DivideSlice(slice, bestIndex, ori);
                    slicesQueue.Enqueue(bestDivision.Item1);
                    slicesQueue.Enqueue(bestDivision.Item2);
                }
            }
        }
        HashSet<Tuple<int, int>> setPos = new HashSet<Tuple<int, int>>();

        void FillEmptySpaces()
        {
            for (int x = 0; x < pizza.NumCols; x++)
            {
                for (int y = 0; y < pizza.NumRows; y++)
                {
                    if(!IsCellOccupied(x, y))
                    {
                        Slice slice = FillSlice(x, y, Slices);
                        if(slice.Size > 1)
                        {
                            Slices.Add(slice);
                            foreach (var pos in slice.Positions)
                                setPos.Add(pos);
                        }
                    }
                }
            }
        }

        Slice FillSlice(int initX, int initY, List<Slice> existingSlices)
        {
            Slice slice = new Slice(initX, initX, initY, initY);
            for (int x = initX; x < initX + pizza.MaxCells; x++)
            {
                for (int y = initY; y < initY + pizza.MaxCells; y++)
                {
                    if ((y - initY) * (x - initX) > pizza.MaxCells || x >= pizza.NumCols || y >= pizza.NumRows)
                    {
                        break;
                    }
                    var newSlice = new Slice(initX, x, initY, y);
                    if(newSlice.Size > slice.Size && newSlice.Size <= pizza.MaxCells && pizza.HasMinimumIngredients(newSlice) && !SliceOverlaps(newSlice))
                    {
                        slice = newSlice;
                    }
                }
            }
            return slice;
        }

        Boolean IsCellOccupied(int x, int y)
        {
            if (setPos.Contains(new Tuple<int, int>(y, x)))
            {
                return true;
            }
            return false;
        }

        Boolean SliceOverlaps(Slice slice)
        {
            foreach(var pos in slice.Positions)
            {
                if (setPos.Contains(pos))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
