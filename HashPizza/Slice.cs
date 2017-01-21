using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    class Slice
    {
        int left;
        int right;
        int top;
        int bottom;

        int getSize()
        {
            return (right - left) * (top - bottom);
        }
    }
}
