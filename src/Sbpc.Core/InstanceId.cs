using System.Threading;

namespace Sbpc.Core;

public static class InstanceId
{
    private static int _nextId = 2000000000;

    public static int GetNextId()
    {
        int id = Interlocked.Increment(ref _nextId);
        return id;
    }
}