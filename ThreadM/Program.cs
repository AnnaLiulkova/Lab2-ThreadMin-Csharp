using System;
using System.Diagnostics;

class Program
{
     static void Main(string[] args)
    {
        Console.Write("Enter array size: ");
        int dim = int.Parse(Console.ReadLine());

        Console.Write("Enter the desired number of threads: ");
        int threadNum = int.Parse(Console.ReadLine());

        Console.WriteLine("Generating array...");
        ArrClass arrClass = new ArrClass(dim, threadNum);

        Stopwatch stopwatch = Stopwatch.StartNew();

        Console.WriteLine("Starting multi-threaded search...\n");
        arrClass.ThreadMin();

        stopwatch.Stop();
        Console.WriteLine($"Total execution time: {stopwatch.ElapsedMilliseconds} ms");
    }
}