/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 18:38
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
	/// Description of StartNeoloadRecording.
	/// </summary>
	[TestModule("C5B1B0EA-FFAA-4EC8-B37B-472B73F45291", ModuleType.UserCode, 1)]
	public class StartNeoloadRecording : ITestModule
	{
		
		string _userPath = "";
		[TestVariable("216b4936-592f-40c0-8c8b-6fe68fce74bb")]
		public string userPath
		{
			get { return _userPath; }
			set { _userPath = value; }
		}
		
		[TestVariable("29B0EB4D-D82E-4DDA-8ABB-355734B346D2")]
		public string Interval { get; set; }
		
		[TestVariable("7E6AAB81-CA2F-4084-A2B2-C0B8792122A1")]
		public string Timeout { get; set; }
		
		string _updateUserPath = "true";
		[TestVariable("8a8f3b2c-1b8e-458b-b5aa-a91e0042e99a")]
		public string updateUserPath
		{
			get { return _updateUserPath; }
			set { _updateUserPath = value; }
		}
		
		string _userAgentString = "";
		[TestVariable("ecfc6410-9ba8-4352-85be-b370b0391967")]
		public string userAgentString
		{
			get { return _userAgentString; }
			set { _userAgentString = value; }
		}
		
		
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public StartNeoloadRecording()
		{
			this.Interval = "00:00:10";
			this.Timeout = "00:01:00";
		}

		/// <summary>
		/// Performs the playback of actions in this module.
		/// </summary>
		/// <remarks>You should not call this method directly, instead pass the module
		/// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
		/// that will in turn invoke this method.</remarks>
		void ITestModule.Run()
		{
			if (string.IsNullOrWhiteSpace(this.userPath))
			{
				throw new InvalidOperationException("User path is required.");
			}
			
			try
			{
				const string fmt = @"hh\:mm\:ss";
				var timeout = TimeSpan.ParseExact(this.Timeout, fmt, CultureInfo.InvariantCulture);
				var interval = TimeSpan.ParseExact(this.Interval, fmt, CultureInfo.InvariantCulture);

				if (timeout < interval)
				{
					throw new ArgumentException(string.Format("The given timeout of '{0}' is smaller than the interval with a value of '{1}', but interval has to be smaller than timeout.",
					                                          timeout.ToString(fmt), interval.ToString(fmt)));
				}
				var wrapper = NeoloadDesignAPIWrapper.GetNeoloadDesignTimeWrapper;
				wrapper.startRecording(userPath, Convert.ToBoolean(updateUserPath), timeout, interval, userAgentString);
			}
			catch (FormatException ex)
			{
				throw new Exception("'Timeout' or 'Interval' was specified with invalid format. Please use the format 'hh:mm:ss' e.g. '00:01:10' for one minute and ten seconds." + ex);
			}
		}
	}
}
