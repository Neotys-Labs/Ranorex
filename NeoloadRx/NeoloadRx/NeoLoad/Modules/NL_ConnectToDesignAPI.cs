/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 17:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Ranorex.Core.Testing;

namespace NeoloadDesignTest.lib
{
  /// <summary>
  /// Description of InitDesignApiConnection.
  /// </summary>
  [TestModule("DDB5D95F-5258-4E13-8E97-2D8ED0D73741", ModuleType.UserCode, 1)]
  public class NL_ConnectToDesignAPI : ITestModule
  {
    
    [TestVariable("2cb2bf6d-616a-4fd3-afc5-4a301fb94338")]
    public string DesignApiUri{get; set;}
    
    string _ApiKey = "null";
    [TestVariable("4cc6f8c7-1ccf-43af-895e-8778d8ebedc3")]
    public string ApiKey
    {
    	get { return _ApiKey; }
    	set { _ApiKey = value; }
    }
    
    public NL_ConnectToDesignAPI()
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
      if (string.IsNullOrWhiteSpace(this.DesignApiUri))
      {
      	throw new InvalidOperationException("No design API URL provided. Cannot connect to NeoLoad server provided.");
      }
    	
      var wrapper = NeoloadDesignAPIWrapper.GetNeoloadDesignTimeWrapper;
      wrapper.ConnectToDesignApi(DesignApiUri, ApiKey);
    }
  }
}
