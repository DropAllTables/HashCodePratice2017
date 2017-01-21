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

        public float GetTomatoMushroomBalance()
        {
            int totalCells = NumRows * NumCols;
            int tomatos = Ingredients.Count(i => i == Ingredient.Tomato);

            return tomatos / (float)totalCells;
        }

        public int GetNumIngredients(Ingredient ingredient, Slice slice)
        {
            int total = 0;

            for (int y = Math.Max(slice.Top, 0); y < Math.Min(slice.Bottom, NumRows - 1); y++)
            {
                int startX = Math.Max(slice.Left, 0);
                int position = y * NumCols + startX;

                for (int x = startX; x < Math.Min(slice.Right, NumCols - 1); x++)
                {
                    total += Ingredients[position++] == ingredient ? 1 : 0;
                }
            }

            return total;
        }

        internal bool HasMinimumIngredients(Slice slice)
        {
            int numTomatoes = 0;
            int numMushrooms = 0;

            foreach (var position in slice.Positions)
            {
                if (Ingredients[GetPosition(position.Item1, position.Item2)] == Ingredient.Tomato)
                {
                    ++numTomatoes;
                } else
                {
                    ++numMushrooms;
                }
            }

            return numTomatoes >= MinIngredients && numMushrooms >= MinIngredients;
        }

        public int SliceScore(Slice slice)
        {
            if(!HasMinimumIngredients(slice))
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
