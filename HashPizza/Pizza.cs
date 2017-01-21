using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class Pizza
    {
        public int MinIngredients;
        public int MaxCells;
        public int NumRows;
        public int NumCols;
        public Ingredient[] Ingredients;

        public int GetPosition(int row, int col)
            => row * NumCols + col;

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(NumRows);
            builder.Append(' ');
            builder.Append(NumCols);
            builder.Append(' ');
            builder.Append(MinIngredients);
            builder.Append(' ');
            builder.Append(MaxCells);
            builder.AppendLine();

            for (int row = 0; row < NumRows; ++row)
            {
                for (int col = 0; col < NumCols; ++col)
                {
                    var ingredient = Ingredients[GetPosition(row, col)];
                    builder.Append(ingredient == Ingredient.Tomato ? 'T' : 'M');
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public Ingredient[] GetSlice(Slice slice)
        {
            LinkedList<Ingredient> list = new LinkedList<Ingredient>();
            for(int x = slice.Left; x < slice.Right; x++)
            {
                for (int y = slice.Top; y < slice.Bottom; y++)
                {
                    list.AddLast(Ingredients[GetPosition(x, y)]);
                }
            }
            return list.ToArray();
        }

        public int GetNumIngredients(Ingredient ingredient, Slice slice)
        {
            var ings = GetSlice(slice);
            return ings.Count(x => x == ingredient);
        }

        public int SliceScore(Slice slice)
        {
            if(GetNumIngredients(Ingredient.Tomato, slice) < MinIngredients)
            {
                return -1;
            }

            if (GetNumIngredients(Ingredient.Mushroom, slice) < MinIngredients)
            {
                return -1;
            }

            if(slice.Size > MaxCells)
            {
                return -1;
            }
            return slice.Size;
        }
    }
}
