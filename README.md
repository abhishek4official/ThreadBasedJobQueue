# ThreadBasedJobQueue


 IThreadJobQueue obj=new ThreadJobQueue(maxParallelizationCount:4);

 maxParallelizationCount: Limit No of threads at given time  
 maxQueueLength: Limits Queue Length 

 # Call Sample
 obj.Queue(futureTask: async ()=>{await RunMyJob(parm);  });

