/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 18:46
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

namespace NeoloadDesignTest.NeoloadDesignLib
{
  /// <summary>
  /// Description of StopNeoloadRecording_Extended.
  /// </summary>
  [TestModule("7EF44A78-0C81-44FA-9E48-E000E4EA8D3E", ModuleType.UserCode, 1)]
  public class NL_StopRecording_Extended : ITestModule
  {
    
    string _timeout = "1200";
    [TestVariable("254dea08-0bea-418a-b991-caf6af62c065")]
    public string timeout
    {
      get { return _timeout; }
      set { _timeout = value; }
    }
    
    string _frameworkParameterSearch = "true";
    [TestVariable("118250c0-b44d-470b-9d38-05dac7c8d2c2")]
    public string frameworkParameterSearch
    {
      get { return _frameworkParameterSearch; }
      set { _frameworkParameterSearch = value; }
    }
    
    string _genericParameterSearch = "true";
    [TestVariable("b70ee6fd-a1c7-414e-b7fc-e7a208fd2876")]
    public string genericParameterSearch
    {
      get { return _genericParameterSearch; }
      set { _genericParameterSearch = value; }
    }
    
    string _deleteExistingRecording = "false";
    [TestVariable("e5222417-ee0a-4a47-937c-e79a3879a89b")]
    public string deleteExistingRecording
    {
      get { return _deleteExistingRecording; }
      set { _deleteExistingRecording = value; }
    }
    
    string _includeVariablesInUserpathMerge = "true";
    [TestVariable("19d18a4f-3bd6-4789-af6e-6abc2ff3d3f5")]
    public string includeVariablesInUserpathMerge
    {
      get { return _includeVariablesInUserpathMerge; }
      set { _includeVariablesInUserpathMerge = value; }
    }
    
    string _matchingThreshold = "60";
    [TestVariable("939de9a7-61d5-4659-b4af-380bde90fda5")]
    public string matchingThreshold
    {
      get { return _matchingThreshold; }
      set { _matchingThreshold = value; }
    }
    
    string _updateSharedContainers = "false";
    [TestVariable("28e3169c-46a1-4a33-864d-21a390357583")]
    public string updateSharedContainers
    {
      get { return _updateSharedContainers; }
      set { _updateSharedContainers = value; }
    }
    
    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public NL_StopRecording_Extended()
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
      wrapper.stopRecording(Convert.ToInt32(timeout), Convert.ToBoolean(frameworkParameterSearch), Convert.ToBoolean(genericParameterSearch),
                            Convert.ToBoolean(deleteExistingRecording), Convert.ToBoolean(includeVariablesInUserpathMerge),
                            Convert.ToInt32(matchingThreshold), Convert.ToBoolean(updateSharedContainers));
    }
  }
}
