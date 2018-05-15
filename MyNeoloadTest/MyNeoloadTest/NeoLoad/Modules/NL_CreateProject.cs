/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 18:25
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
  /// Description of CreateNewNeoloadProject.
  /// </summary>
  [TestModule("B5837194-4007-495C-B9CD-C635C8B09B95", ModuleType.UserCode, 1)]
  public class NL_CreateProject : ITestModule
  {
    
    string _projectName = "";
    [TestVariable("0b7c3530-3314-4387-9296-d71f8e33eda5")]
    public string projectName
    {
      get { return _projectName; }
      set { _projectName = value; }
    }
    
    string _directoryPath = "";
    [TestVariable("02e90948-e96e-4e8c-8a48-331b1f74ef10")]
    public string directoryPath
    {
      get { return _directoryPath; }
      set { _directoryPath = value; }
    }
    
    string _overwriteExisting = "false";
    [TestVariable("caa5ce38-0ce3-43cd-9e0c-60229916ce29")]
    public string overwriteExisting
    {
      get { return _overwriteExisting; }
      set { _overwriteExisting = value; }
    }
    
    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public NL_CreateProject()
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
      wrapper.createNewNeoloadProject(projectName, directoryPath, Boolean.Parse(overwriteExisting));
    }
  }
}
