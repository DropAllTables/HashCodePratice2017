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
            var inputs = new string[] { "small", "example", "medium", "big" };

            Solution solution = null;
            foreach (var input in inputs)
            {
                var pizza = PizzaReader.ReadPizza($"../../../inputs/{input}.in");

                solution = GreedyPizza.Solve(pizza);
                //NaiveOptimizer.Optimize(solution);
                //SliceBreakerOptimizer.Optimize(solution);
                Console.WriteLine(solution.Score);
                Console.WriteLine("Efficiency: " + solution.Score / (float) (pizza.NumRows * pizza.NumCols));
                SolutionWriter.Write(solution, $"result-{input}.txt");
            }

            System.Windows.Forms.Application.Run(new Visualizer(solution));
        }
    }
}
