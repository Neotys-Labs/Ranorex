/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 18:24
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
  /// Description of OpenNeoloadProject.
  /// </summary>
  [TestModule("7436648F-342D-444D-B2DD-F1347E262BDB", ModuleType.UserCode, 1)]
  public class NL_OpenProject : ITestModule
  {
    
    
    string _filePath = "";
    [TestVariable("8a866ccd-253d-4886-8575-0cdd6c304cde")]
    public string filePath
    {
      get { return _filePath; }
      set { _filePath = value; }
    }
    
    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public NL_OpenProject()
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
        wrapper.openNeoloadProject(filePath);
    }
  }
}
