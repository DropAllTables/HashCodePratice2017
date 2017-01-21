using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class Slice
    {
        int left;
        int right;
        int top;
        int bottom;

        public int Left
            => left;
        public int Right
            => right;
        public int Width
            => right - left;
        public int Top
            => top;
        public int Bottom
            => bottom;
        public int Height
        => bottom - top;

        public int Size
            => Width * Height;

        public Slice(int left, int right, int top, int bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }

        public IEnumerable<Tuple<int, int>> Positions
        {
            get
            {
                for (int row = Top; row <= Bottom; ++row)
                {
                    for (int col = Left; col <= Right; ++col)
                    {
                        yield return Tuple.Create(row, col);
                    }
                }
            }
        }
    }
}
