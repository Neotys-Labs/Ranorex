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
      var wrapper = NeoloadDesignAPIWrapper.GetNeoloadDesignTimeWrapper;
      wrapper.saveNeoloadProject();
    }
  }
}
