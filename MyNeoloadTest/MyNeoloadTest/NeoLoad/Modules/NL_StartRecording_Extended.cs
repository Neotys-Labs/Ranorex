/*
 * Created by Ranorex
 * User: cbreit
 * Date: 24.02.2017
 * Time: 18:43
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
	/// Description of StartNeoloadRecording_Extended.
	/// </summary>
	[TestModule("0CA7A37E-2C42-41F3-989C-0F5BC0E014BB", ModuleType.UserCode, 1)]
	public class NL_StartRecording_Extended : ITestModule
	{
		string _userPath = "UserPath";
		[TestVariable("216b4936-592f-40c0-7a8b-6fe68fce74bb")]
		public string userPath
		{
			get { return _userPath; }
			set { _userPath = value; }
		}
		
		string _updateUserPath = "true";
		[TestVariable("8a8f3b2c-1b8e-458b-b5bb-a91e0042e99a")]
		public string updateUserPath
		{
			get { return _updateUserPath; }
			set { _updateUserPath = value; }
		}
		
		[TestVariable("29B0EB4D-D82E-4DDA-8ABB-355734B346D2")]
		public string Interval { get; set; }
		
		[TestVariable("7E6AAB81-CA2F-4084-A2B2-C0B8792122A1")]
		public string Timeout { get; set; }
		
		string _userAgentString = "";
		[TestVariable("ecfc6410-9ba8-4352-bbbe-b370b0391967")]
		public string userAgentString
		{
			get { return _userAgentString; }
			set { _userAgentString = value; }
		}
		
		string _isWebSocketProtocol = "false";
		[TestVariable("46be8c80-6db0-45c0-9b1e-a305609d5b98")]
		public string isWebSocketProtocol
		{
			get { return _isWebSocketProtocol; }
			set { _isWebSocketProtocol = value; }
		}
		
		string _isHttp2Protocol = "false";
		[TestVariable("4ed89ec7-53ba-49e0-8e50-fd1de62f87dd")]
		public string isHttp2Protocol
		{
			get { return _isHttp2Protocol; }
			set { _isHttp2Protocol = value; }
		}
		
		string _isAdobeRTMPProtocol = "false";
		[TestVariable("cf624e43-33fa-4287-b53d-12b1bca514f4")]
		public string isAdobeRTMPProtocol
		{
			get { return _isAdobeRTMPProtocol; }
			set { _isAdobeRTMPProtocol = value; }
		}
		
		[TestVariable("f5b2d30e-846a-475b-b796-7e51190429f1")]
        public string addressToExclude { get; set; }
		
		
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public NL_StartRecording_Extended()
		{
			this.Interval = "00:00:05";
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
				wrapper.startRecording(userPath, Convert.ToBoolean(updateUserPath),timeout, interval, userAgentString,
				                       Convert.ToBoolean(isWebSocketProtocol), Convert.ToBoolean(isHttp2Protocol), Convert.ToBoolean(isAdobeRTMPProtocol), addressToExclude);
			}
				catch (FormatException ex)
				{
					throw new Exception("'Timeout' or 'Interval' was specified with invalid format. Please use the format 'hh:mm:ss' e.g. '00:01:10' for one minute and ten seconds." + ex);
				}
			}
		}
	}

