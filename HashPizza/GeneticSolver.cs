using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class GeneticSolver
    {
        public static Solution Solve(Pizza pizza, int numIndividuals, int numIterations, float mutationProbability)
        {
            int rowBits = (int)Math.Ceiling(Math.Log(pizza.NumRows) / Math.Log(2));
            int colBits = (int)Math.Ceiling(Math.Log(pizza.NumCols) / Math.Log(2));
            double maxSize = Math.Min(pizza.MaxCells, Math.Max(pizza.NumRows, pizza.NumCols));
            int sizeBits = (int)(Math.Ceiling(Math.Log(maxSize - 1) / Math.Log(2)));

            // ceil(R * C / H)
            int maxNumSlices = (pizza.NumRows * pizza.NumCols + pizza.MaxCells - 1) / pizza.MaxCells;

            int sliceBits = rowBits + colBits + 2 * sizeBits;
            int genomeBits = sliceBits * maxNumSlices;

            Random random = new Random();

            List<bool[]> individuals = new List<bool[]>();

            // Build initial state
            for (int i = 0; i < numIndividuals; ++i)
            {
                bool[] individual = CreateIndividual(genomeBits, random);

                individuals.Add(individual);
            }

            Solution bestSolutionSoFar = null;
            int bestSolutionValue = int.MinValue;

            for (int it = 0; it < numIterations; ++it)
            {
                PerformMutations(individuals, random, mutationProbability);

                Solution[] solutions = new Solution[numIndividuals];
                for (int i = 0; i < numIndividuals; ++i)
                {
                    solutions[i] = BuildSolution(pizza, individuals[i], rowBits, colBits, sizeBits, maxNumSlices);
                }

                var proposedResult = solutions
                    .WithMax(solution => solution.Score);

                if (proposedResult.Item2 > bestSolutionValue)
                {
                    bestSolutionSoFar = proposedResult.Item1;
                    bestSolutionValue = proposedResult.Item2;
                }
            }

            return bestSolutionSoFar;
        }

        private static void PerformMutations(List<bool[]> individuals, Random random, float mutationProbability)
        {
            foreach (var individual in individuals)
            {
                for (int bit = 0; bit < individual.Length; ++bit)
                {
                    if (mutationProbability > random.NextDouble())
                    {
                        individual[bit] = !individual[bit];
                    }
                }
            }
        }

        private static Solution BuildSolution(Pizza pizza, bool[] individual, int rowBits, int colBits, int sizeBits, int maxNumSlices)
        {
            var solution = new Solution(pizza);

            int sliceSize = rowBits + colBits + 2 * sizeBits;

            for (int sliceId = 0; sliceId < maxNumSlices; ++sliceId)
            {
                int sliceStart = sliceId * sliceSize;

                int row = 0;
                int bit = sliceStart;
                for (; bit < sliceStart + rowBits; ++bit)
                {
                    row <<= 1;
                    row += individual[bit] ? 1 : 0;
                }

                int col = 0;
                for (; bit < sliceStart + rowBits + colBits; ++bit)
                {
                    col <<= 1;
                    col += individual[bit] ? 1 : 0;
                }

                int width = 0;
                for (; bit < sliceStart + rowBits + colBits + sizeBits; ++bit)
                {
                    width <<= 1;
                    width += individual[bit] ? 1 : 0;
                }

                int height = 0;
                for (; bit < sliceStart + rowBits + colBits + sizeBits; ++bit)
                {
                    height <<= 1;
                    height += individual[bit] ? 1 : 0;
                }

                ++width;
                ++height;

                if (row < 0 | col < 0)
                {
                    continue;
                }

                if (row + height > pizza.NumRows | col + width > pizza.NumCols)
                {
                    continue;
                }

                var slice = new Slice(col, col + width - 1, row, row + height - 1);
                if (slice.Size < pizza.MaxCells && pizza.HasMinimumIngredients(slice) && solution.ExtraSliceFits(slice))
                {
                    solution.AddSlice(slice);
                }
            }

            return solution;
        }

        private static bool[] CreateIndividual(int genomeBits, Random random)
        {
            bool[] individual = new bool[genomeBits];
            for (int j = 0; j < genomeBits; ++j)
            {
                individual[j] = random.Next(2) == 1;
            }

            return individual;
        }
    }
}
