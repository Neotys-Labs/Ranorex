///////////////////////////////////////////////////////////////////////////////////////////////////
//
// This file is part of the  R A N O R E X  Project.
// http://www.ranorex.com
//
///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using Ranorex.Core.Testing;

namespace Ranorex.NeoLoad
{
    /// <summary>
    /// Stops a NeoLoad test.
    /// </summary>
    [TestModule("861A8070-224A-47BB-91A1-920E12DA1065", ModuleType.UserCode, 1)]
    public class NL_StopTest : ITestModule
    {
        // For testing, make it mockable
        internal static INeoloadApi api = NeoloadApi.Instance;
       
       string _forceStop = "false";
       [TestVariable("1e6bf3b2-e1a7-404c-8fa9-256a7de2b1b6")]
       public string forceStop
       {
       	get { return _forceStop; }
       	set { _forceStop = value; }
       }
        
        /// <summary>
        /// Timeout for the stop operation in the format 'hh:mm:ss'.
        /// </summary>
        [TestVariable("7E6AAB81-CA2F-4084-A2B2-C0B8792122A1")]
        public TimeSpan Timeout { get; set; }
        /// <summary>
        /// Check interval for operations that can timeout in the format 'hh:mm:ss'.
        /// This interval needs to be smaller than the timeout.
        /// </summary
        [TestVariable("29B0EB4D-D82E-4DDA-8ABB-355734B346D2")]
        public TimeSpan Interval { get; set; }

        public NL_StopTest()
        {
        	this.Timeout = TimeSpan.FromSeconds(10);
        	this.Interval = TimeSpan.FromSeconds(1);
        }
        
        void ITestModule.Run()
        {
            api.StopNeoLoadTest(this.Timeout, this.Interval, Boolean.Parse(forceStop));
        }
    }
}
