using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class Solution
    {
        public Pizza pizza;
        List<Slice> slices = new List<Slice>();
        bool[] positions;

        public Solution(Pizza pizza)
        {
            this.pizza = pizza;
            this.positions = new bool[pizza.NumRows * pizza.NumCols];
        }

        public void AddSlice(Slice slice)
        {
            slices.Add(slice);
            AddSlicePositions(slice);
        }

        public void AddSlicePositions(Slice slice)
        {
            for (int row = slice.Top; row <= slice.Bottom; ++row)
            {
                for (int col = slice.Left; col <= slice.Right; ++col)
                {
                    positions[pizza.GetPosition(row, col)] = true;
                }
            }
        }

        public bool ExtraSliceFits(Slice extraSlice)
        {
            for (int row = extraSlice.Top; row <= extraSlice.Bottom; ++row)
            {
                for (int col = extraSlice.Left; col <= extraSlice.Right; ++col)
                {
                    if (positions[pizza.GetPosition(row, col)])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool UsesPosition(int row, int col)
            => positions[pizza.GetPosition(row, col)];

        public int Score
            => slices.Sum(slice => slice.Size);

        public List<Slice> Slices
            => slices;
    }
}
