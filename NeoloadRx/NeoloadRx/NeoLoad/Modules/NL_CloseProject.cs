/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 18:28
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
  /// Description of CloseNeoloadProject.
  /// </summary>
  [TestModule("542010D5-9D0D-46DF-91D9-CEECCAFBE9F8", ModuleType.UserCode, 1)]
  public class NL_CloseProject : ITestModule
  {
    
    string _saveProject = "true";
    [TestVariable("c8ab3ec1-06dd-40dd-84dd-56856a129743")]
    public string saveProject
    {
      get { return _saveProject; }
      set { _saveProject = value; }
    }
    
    string _forceStop = "false";
    [TestVariable("ae358bd4-2ea5-4153-9beb-fe1ad77c27e3")]
    public string forceStop
    {
      get { return _forceStop; }
      set { _forceStop = value; }
    }
    
    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public NL_CloseProject()
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
      wrapper.closeNeoloadProject(Convert.ToBoolean(saveProject), Convert.ToBoolean(forceStop));
    }
  }
}
