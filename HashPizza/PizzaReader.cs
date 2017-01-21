using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HashPizza
{
    public class PizzaReader
    {
        public static Pizza ReadPizza(string filename)
        {
            using (var stream = new StreamReader(File.OpenRead(filename)))
            {
                var pizza = new Pizza();

                string header = stream.ReadLine();
                var components = header.Split(' ');
                pizza.NumRows = int.Parse(components[0]);
                pizza.NumCols = int.Parse(components[1]);
                pizza.MinIngredients = int.Parse(components[2]);
                pizza.MaxCells = int.Parse(components[3]);

                pizza.Ingredients = new Ingredient[pizza.NumRows * pizza.NumCols];

                return pizza;
            }
        }
    }
}
