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
            Console.WriteLine(PizzaReader.ReadPizza("../../../inputs/small.in"));
            Console.Read();
        }
    }
}
