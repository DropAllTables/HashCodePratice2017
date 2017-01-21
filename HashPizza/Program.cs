using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "small";

            var pizza = PizzaReader.ReadPizza($"../../../inputs/{input}.in");

            var solution = new Solution();
            solution.Slices.Add(new Slice(0, 1, 2, 3));
            SolutionWriter.Write(solution, $"result-{input}.txt");
        }
    }
}
