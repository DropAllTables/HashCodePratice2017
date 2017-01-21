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

            foreach (var input in inputs)
            {
                var pizza = PizzaReader.ReadPizza($"../../../inputs/{input}.in");

                Solution solution = new GreedyPizza(pizza);
                var startSolutions = new List<Solution> {
                    solution
                };
                solution = GeneticSolver.Solve(pizza, startSolutions, 30, 20, 0.01f, 0.05f, 20);
                Console.WriteLine(solution.Score);
                SolutionWriter.Write(solution, $"result-{input}.txt");
            }
            Console.Read();
        }
    }
}
