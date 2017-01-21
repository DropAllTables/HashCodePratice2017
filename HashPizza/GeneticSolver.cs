using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPizza
{
    public class GeneticSolver
    {
        public static Solution Solve(Pizza pizza,
            List<Solution> startSolutions,
            int numIndividuals, int numIterations, float mutationProbability, float crossOverProbability,
            int survivalsPerGeneration)
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
            if (startSolutions.Count == 0)
            {
                for (int i = 0; i < numIndividuals; ++i)
                {
                    bool[] individual = CreateIndividual(pizza, rowBits, colBits, sizeBits, maxNumSlices, random);

                    individuals.Add(individual);
                }
            }
            else
            {
                for (int i = 0; i < numIndividuals; ++i)
                {
                    bool[] individual = CreateIndividualFromSolution(pizza, rowBits, colBits, sizeBits, maxNumSlices, startSolutions[i % startSolutions.Count]);

                    individuals.Add(individual);
                }
            }

            var bestInitial = startSolutions.WithMax(sol => sol.Score);

            Solution bestSolutionSoFar = bestInitial.Item1;
            int bestSolutionValue = bestInitial.Item2;

            Console.WriteLine("Best initial " + bestSolutionValue);

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
                int maxScore = proposedResult.Item2;

                // Selection
                individuals = solutions.Select((solution, index) => Tuple.Create(index, solution.Score))
                    .OrderByDescending(proposal => proposal.Item2)
                    .Select(proposal => individuals[proposal.Item1])
                    .Take(survivalsPerGeneration)
                    .ToList();

                while (individuals.Count < numIndividuals)
                {
                    // Reproduce
                    individuals.Add((bool[]) individuals[random.Next(individuals.Count)].Clone());
                }

                // Cross-over
                for (int i = 0; i < numIndividuals; i += 2)
                {
                    bool[] genome1 = individuals[i];
                    bool[] genome2 = individuals[i + 1];

                    for (int bit = 0; bit < genome1.Length; ++bit)
                    {
                        if (crossOverProbability > random.NextDouble())
                        {
                            bool bit1 = genome1[bit];
                            genome1[bit] = genome2[bit];
                            genome2[bit] = bit1;
                        }
                    }
                }

                Console.WriteLine("> " + maxScore);

                if (maxScore > bestSolutionValue)
                {
                    bestSolutionSoFar = proposedResult.Item1;
                    bestSolutionValue = maxScore;
                }
            }

            return bestSolutionSoFar;
        }

        private static bool[] CreateIndividual(Pizza pizza, int rowBits, int colBits, int sizeBits, int maxNumSlices, Random random)
        {
            Solution solution = GenerateRandomSolution(pizza, maxNumSlices, random);
            return CreateIndividualFromSolution(pizza, rowBits, colBits, sizeBits, maxNumSlices, solution);
        }

        private static bool[] CreateIndividualFromSolution(Pizza pizza, int rowBits, int colBits, int sizeBits, int maxNumSlices, Solution solution)
        {
            var bitsPerSlice = rowBits + colBits + sizeBits * 2;
            bool[] genome = new bool[bitsPerSlice * maxNumSlices];
            for (int sliceId = 0; sliceId < solution.Slices.Count; ++sliceId)
            {
                var slice = solution.Slices[sliceId];

                int row = slice.Top;
                int col = slice.Left;
                int width = slice.Width - 1;
                int height = slice.Height - 1;

                int start = sliceId * bitsPerSlice;
                for (int bitOffset = 0; bitOffset < rowBits; ++bitOffset)
                {
                    bool value = (row & (1 << (rowBits - bitOffset - 1))) != 0;
                    
                    genome[start + bitOffset] = value;
                }

                start += rowBits;
                for (int bitOffset = 0; bitOffset < colBits; ++bitOffset)
                {
                    bool value = (col & (1 << (colBits - bitOffset - 1))) != 0;
                    
                    genome[start + bitOffset] = value;
                }

                start += colBits;
                for (int bitOffset = 0; bitOffset < sizeBits; ++bitOffset)
                {
                    bool value = (width & (1 << (sizeBits - bitOffset - 1))) != 0;
                    
                    genome[start + bitOffset] = value;
                }

                start += sizeBits;
                for (int bitOffset = 0; bitOffset < sizeBits; ++bitOffset)
                {
                    bool value = (height & (1 << (sizeBits - bitOffset - 1))) != 0;
                    
                    genome[start + bitOffset] = value;
                }
            }

            return genome;
        }

        private static Solution GenerateRandomSolution(Pizza pizza, int maxNumSlices, Random random)
        {
            var solution = new Solution(pizza);
            for (int sliceId = 0; sliceId < maxNumSlices; ++sliceId)
            {
                int row = random.Next(pizza.NumRows);
                int col = random.Next(pizza.NumCols);
                int width = random.Next(pizza.MinIngredients * 2, pizza.MaxCells);
                int height = random.Next(pizza.MinIngredients * 2, pizza.MaxCells);

                var slice = new Slice(col, col + width - 1, row, row + height - 1);
                if (SliceAdditionValid(pizza, solution, slice))
                {
                    solution.AddSlice(slice);
                }
            }

            return solution;
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
                for (; bit < sliceStart + rowBits + colBits + 2* sizeBits; ++bit)
                {
                    height <<= 1;
                    height += individual[bit] ? 1 : 0;
                }

                ++width;
                ++height;

                var slice = new Slice(col, col + width - 1, row, row + height - 1);
                if (SliceAdditionValid(pizza, solution, slice))
                {
                    solution.AddSlice(slice);
                }
            }

            return solution;
        }

        private static bool SliceAdditionValid(Pizza pizza, Solution solution, Slice slice)
        {
            if (slice.Left < 0 | slice.Top < 0)
            {
                return false;
            }

            if (slice.Bottom >= pizza.NumRows | slice.Right >= pizza.NumCols)
            {
                return false;
            }

            return slice.Size <= pizza.MaxCells && pizza.HasMinimumIngredients(slice) && solution.ExtraSliceFits(slice);
        }
    }
}
