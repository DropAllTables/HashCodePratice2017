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
    }
}
