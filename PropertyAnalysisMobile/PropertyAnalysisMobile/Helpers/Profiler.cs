using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyAnalysisMobile.Helpers
{
    public class Profiler : IDisposable
    {
        Stopwatch stopWatch;
        string message;

        public Profiler (string methodName)
        {
            message = methodName;
            stopWatch = new Stopwatch();
            stopWatch.Start();
            Debug.WriteLine("Start of Method: {0}", methodName);
        }
        public void Dispose()
        {
            if(stopWatch == null)
            {
                return;
            }

            stopWatch.Stop();
            Debug.WriteLine("Method Finished: {0}", message);
            Debug.WriteLine("Time Taken: {0}", stopWatch.Elapsed.TotalSeconds);
        }
    }
}
