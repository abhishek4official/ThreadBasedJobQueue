using System;
using System.Threading;
using System.Threading.Tasks;
using abhi.TaskQueueLib;

namespace ThreadBasedJobQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            IThreadJobQueue obj=new ThreadJobQueue(maxParallelizationCount:4);
             //Test Job Queue
            //var t = new JobQueue(maxParallelizationCount: 1);
            Random rnd = new Random();
            
            for(int i=0;i<=100;i++){
                int Wait=rnd.Next(1,10) * 1000;
                var parm=new MyParameters{delayTime=Wait,JobNo=i};
                obj.Queue(futureTask: async ()=>{await RunMyJob(parm);  });
            }
            Console.WriteLine("Press Enter to Know Status");
           do{
             Console.WriteLine("\nStatus::\nIsQueueRunning: "+obj.isQueueRunning()+ " IsQueue: "+ obj.GetQueueCount()+ " Running Job:"+obj.GetRunningCount())  ;//obj.GetQueueCount();
           }while(Console.ReadLine()!="Exit");
        }
        public static async  Task RunMyJob(MyParameters myParameters)
        {

            

            DateTime Start=DateTime.Now;

            Thread.Sleep(myParameters.delayTime); 

            Console.WriteLine("Job No: "+myParameters.JobNo+"\n DelayTime "+myParameters.delayTime+" StartTime: " + Start +" End Time: "+DateTime.Now);

        }
    }
    public class MyParameters{
        public int JobNo {get;set;}
        public int delayTime{get;set;}
    }
}
