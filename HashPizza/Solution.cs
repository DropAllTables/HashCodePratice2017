using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class Solution
    {
        public List<Slice> Slices = new List<Slice>();

        public bool ExtraSliceFits(Pizza pizza, Slice extraSlice)
        {
            bool[] positions = new bool[pizza.NumRows * pizza.NumCols];

            foreach (var slice in Slices)
            {
                foreach (var position in slice.Positions)
                {
                    positions[pizza.GetPosition(position.Item1, position.Item2)] = true;
                }
            }

            foreach (var position in extraSlice.Positions)
            {
                if (positions[pizza.GetPosition(position.Item1, position.Item2)])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
