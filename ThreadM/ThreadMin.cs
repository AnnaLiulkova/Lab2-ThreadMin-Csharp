using System.Diagnostics;
public class ThreadMin
{
    private readonly int startIndex;
    private readonly int finishIndex;
    private readonly ArrClass arrClass;
    private readonly string name;

    public ThreadMin(int startIndex, int finishIndex, ArrClass arrClass, string name)
    {
        this.startIndex = startIndex;
        this.finishIndex = finishIndex;
        this.arrClass = arrClass;
        this.name = name;
    }

    public void Run()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        int[] result = arrClass.PartMin(startIndex, finishIndex);

        stopwatch.Stop();
        double duration = stopwatch.Elapsed.TotalMilliseconds;

        arrClass.CollectMin(result[0], result[1], startIndex, finishIndex, duration, name);

        arrClass.IncThreadCount();
    }
}
