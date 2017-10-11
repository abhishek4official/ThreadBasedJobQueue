
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
namespace abhi.TaskQueueLib

{
    public class ThreadJobQueue : IThreadJobQueue
    {
        private readonly ConcurrentQueue<Func<Task>> _processingQueue = new ConcurrentQueue<Func<Task>>();
        private readonly ConcurrentDictionary<int, Task> _runningTasks = new ConcurrentDictionary<int, Task>();
        private readonly int _maxParallelizationCount;
        private readonly int _maxQueueLength;
        private TaskCompletionSource<bool> _tscQueue = new TaskCompletionSource<bool>();
        private bool _isQueueRunning;
        public ThreadJobQueue(int? maxParallelizationCount = null, int? maxQueueLength = null)
        {
            _maxParallelizationCount = maxParallelizationCount ?? int.MaxValue;
            _maxQueueLength = maxQueueLength ?? int.MaxValue;
            _isQueueRunning = false;
        }
        /// EnQueue Tasks
        public bool Queue(Func<Task> futureTask)
        {
            if (_processingQueue.Count < _maxQueueLength)
            {
                _processingQueue.Enqueue(futureTask);
                if (!_isQueueRunning)
                {
                    Console.WriteLine("Restarting queue: "+ DateTime.Now);
                    this.ProcessBackground();
                }
                else
                Console.WriteLine("Queue allready running: " + DateTime.Now);
                return true;
            }
            return false;
        }
        /// Gets you the count of Processes pending in queue
        public int GetQueueCount()
        {
            return _processingQueue.Count;
        }
        /// Gets you the count of current running Processes(threads) in queue
        public int GetRunningCount()
        {
            return _runningTasks.Count;
        }
        public bool isQueueRunning(){
            return _isQueueRunning;
        }
        /// Starts processing of threads
        public async Task Process()
        {
            var t = _tscQueue.Task;
            StartTasks();
            await t;
        }
        /// Runs all processes in background
        public void ProcessBackground(Action<Exception> exception = null) {
            _isQueueRunning = true;
            System.Threading.Tasks.Task.Run(()=>{Process();}).ContinueWith(t => { if (exception != null) { exception.Invoke(t.Exception); }; }, TaskContinuationOptions.OnlyOnFaulted);
        }
        
        /// Recursive function controles and process the thread
        private void StartTasks()
        {
            
            var startMaxCount = _maxParallelizationCount - _runningTasks.Count;
            for (int i = 0; i < startMaxCount; i++)
            {
                Func<Task> futureTask;
                if (!_processingQueue.TryDequeue(out futureTask))
                {
                    // Queue is most likely empty
                    break;
                }

                var t = Task.Run(futureTask);
                if (!_runningTasks.TryAdd(t.GetHashCode(), t))
                {
                    throw new Exception("Should not happen, hash codes are unique");
                }

                t.ContinueWith((t2) =>
                {
                    Task _temp;
                    if (!_runningTasks.TryRemove(t2.GetHashCode(), out _temp))
                    {
                        throw new Exception("Should not happen, hash codes are unique");
                    }

                    // Continue the queue processing
                    StartTasks();
                });
            }

            if (_processingQueue.IsEmpty && _runningTasks.IsEmpty)
            {
                // Interlocked.Exchange might not be necessary
                var _oldQueue = Interlocked.Exchange(
                    ref _tscQueue, new TaskCompletionSource<bool>());
                _oldQueue.TrySetResult(true);

                _isQueueRunning = false;
            }
        }
    }
}