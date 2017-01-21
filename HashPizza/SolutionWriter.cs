using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HashPizza
{
    public class SolutionWriter
    {
        public static void Write(Solution solution, string filename)
        {
            using (var writer = new StreamWriter(File.OpenWrite(filename)))
            {
                Write(solution, writer);
            }
        }

        public static void Write(Solution solution, StreamWriter writer)
        {
            writer.WriteLine(solution.Slices.Count);
            foreach (var slice in solution.Slices)
            {
                writer.WriteLine($"{slice.Top} {slice.Left} {slice.Bottom} {slice.Right}");
            }
        }
    }
}
