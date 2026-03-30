using System;
using System.Threading;
public class ArrClass
{
    private readonly int dim;
    private readonly int threadNum;
    public readonly int[] arr;

    private int globalMin = int.MaxValue;
    private int globalMinIndex = -1;
    private int threadCount = 0;

    private readonly object _locker = new object();

    public ArrClass(int dim, int threadNum)
    {
        this.dim = dim;
        this.threadNum = threadNum;
        arr = new int[dim];
        Random rand = new Random();

        for (int i = 0; i < dim; i++)
        {
            arr[i] = i;
        }

        int randomNegativeIndex = rand.Next(dim);
        arr[randomNegativeIndex] = -(rand.Next(1000) + 1);
    }

    public int[] PartMin(int startIndex, int finishIndex)
    {
        int localMin = int.MaxValue;
        int localMinIndex = -1;
        for (int i = startIndex; i < finishIndex; i++)
        {
            if (arr[i] < localMin)
            {
                localMin = arr[i];
                localMinIndex = i;
            }
        }
        return new int[] { localMin, localMinIndex };
    }

    public void CollectMin(int min, int index, int startIndex, int finishIndex, double duration, string threadName)
    {
        lock (_locker)
        {
            Console.WriteLine($"Begin -> {threadName}");
            Console.WriteLine($"{threadName} processes indices [{startIndex}; {finishIndex - 1}], local minimum = {min}, index = {index} (Time: {duration} ms)");

            if (min < this.globalMin)
            {
                this.globalMin = min;
                this.globalMinIndex = index;
            }

            Console.WriteLine($"End -> {threadName}");
        }
    }

    public void IncThreadCount()
    {
        lock (_locker)
        {
            threadCount++;
            Monitor.Pulse(_locker); 
        }
    }

    private void WaitForResults()
    {
        lock (_locker)
        {
            while (threadCount < threadNum)
            {
                Monitor.Wait(_locker); 
            }

            Console.WriteLine("\n-RESULT-");
            Console.WriteLine($"Minimum array element: {globalMin}");
            Console.WriteLine($"Index of the minimum element: {globalMinIndex}");
        }
    }

    public void ThreadMin()
    {
        int partSize = dim / threadNum;

        for (int i = 0; i < threadNum; i++)
        {
            int startIndex = i * partSize;
            int finishIndex = (i == threadNum - 1) ? dim : (startIndex + partSize);
            string threadName = $"Thread-{i + 1}";

            ThreadMin threadObj = new ThreadMin(startIndex, finishIndex, this, threadName);
                
            Thread t = new Thread(threadObj.Run);
            t.Name = threadName;
            t.Start();
        }

        WaitForResults();
    }
}
