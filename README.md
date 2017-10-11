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
  
  
 For any query feel free co contact me  
 abhishek4official@gmail.com  
 https://twitter.com/abhishe4work  
 https://in.linkedin.com/in/abhishek4official

