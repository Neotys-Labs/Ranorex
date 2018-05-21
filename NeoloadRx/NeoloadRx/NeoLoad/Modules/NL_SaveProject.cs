/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 18:36
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

namespace NeoloadDesignTest.lib
{
  /// <summary>
  /// Description of SaveNeoloadProject.
  /// </summary>
  [TestModule("23D296C2-D87D-4F67-ACE0-652BDF3F4CFF", ModuleType.UserCode, 1)]
  public class NL_SaveProject : ITestModule
  {
  string _interval = "00:00:10";
  [TestVariable("47b82b06-3aa6-47d2-93e5-f7ede1d50a6c")]
  public string interval
  {
  	get { return _interval; }
  	set { _interval = value; }
  }
  
  string _timeout = "00:05:00";
  [TestVariable("8c5ec291-b9b3-42c1-98c9-1f51ed6cff13")]
  public string timeout
  {
  	get { return _timeout; }
  	set { _timeout = value; }
  }
  
    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public NL_SaveProject()
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
    	try
            {
                const string fmt = @"hh\:mm\:ss";
                var timeout = TimeSpan.ParseExact(this.timeout, fmt, CultureInfo.InvariantCulture);
                var interval = TimeSpan.ParseExact(this.interval, fmt, CultureInfo.InvariantCulture);

                if (timeout < interval)
                {
                    throw new ArgumentException(string.Format("The given timeout of '{0}' is smaller than the interval with a value of '{1}', but interval has to be smaller than timeout.",
                                                              timeout.ToString(fmt), interval.ToString(fmt)));
                }
                var wrapper = NeoloadDesignAPIWrapper.GetNeoloadDesignTimeWrapper;
                wrapper.saveNeoloadProject(timeout, interval);
            }
            catch (FormatException ex)
            {
                throw new Exception("'Timeout' or 'Interval' was specified with invalid format. Please use the format 'hh:mm:ss' e.g. '00:01:10' for one minute and ten seconds." + ex);
            }
      
    }
  }
}
