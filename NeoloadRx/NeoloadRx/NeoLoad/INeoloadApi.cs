using System;
using System.Collections.Generic;

namespace Ranorex.NeoLoad
{
    internal interface INeoloadApi
    {
        void ConnectToRuntimeApi(string runtimeUrl, string apiKey);
        void AddVirtualUsers(string population, int ammount);
        void RemoveVirtualUsers(string population, int ammount);
        void StartNeoLoadTest(string scenario, TimeSpan timeout, TimeSpan interval);
        void StopNeoLoadTest(TimeSpan timeout, TimeSpan interval, Boolean forceStop);
        
    }
    
}
