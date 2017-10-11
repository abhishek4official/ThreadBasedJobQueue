# ThreadBasedJobQueue


 IThreadJobQueue obj=new ThreadJobQueue(maxParallelizationCount:4);

 maxParallelizationCount: Limit No of threads at given time  
 maxQueueLength: Limits Queue Length 

 # Call Sample
 obj.Queue(futureTask: async ()=>{await RunMyJob(parm);  });

 # Nuget Install  
 Package Manager:: Install-Package abhi.TaskQueueLib -Version 1.0.1  
 Or  
 Dotnet CLI:: dotnet add package abhi.TaskQueueLib --version 1.0.1

