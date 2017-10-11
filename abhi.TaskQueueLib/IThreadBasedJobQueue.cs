using System;
using System.Threading.Tasks;

namespace abhi.TaskQueueLib
{
    public interface IThreadJobQueue
    {
         bool Queue(Func<Task> futureTask);
         int GetQueueCount();
         bool isQueueRunning();
         int GetRunningCount();
         Task Process();
         void ProcessBackground(Action<Exception> exception = null);
         
    }
}