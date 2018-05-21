/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 18:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;
using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace NeoloadDesignTest.NeoloadDesignLib
{
    /// <summary>
    /// Description of StopNeoloadRecording.
    /// </summary>
    [TestModule("AFD2CC52-0F56-4372-885C-EC80FB8B19A8", ModuleType.UserCode, 1)]
    public class NL_StopRecording : ITestModule
    {

        string _timeout = "1200";
        [TestVariable("142819e4-fbaa-4a23-a9f5-ffda8e7b6048")]
        public string nl_timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public NL_StopRecording()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
            var wrapper = NeoloadDesignAPIWrapper.GetNeoloadDesignTimeWrapper;
            wrapper.stopRecording(Convert.ToInt32(nl_timeout));
        }
    }
}

