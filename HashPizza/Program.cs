﻿using System;
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

                var solution = GeneticSolver.Solve(pizza, 30, 2, 0.05f);
                Console.WriteLine(solution.Score);
                SolutionWriter.Write(solution, $"result-{input}.txt");
            }
            Console.Read();
        }
    }
}
