/*
 * Created by Ranorex
 * User: cbreit
 * Date: 22.02.2017
 * Time: 16:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using MyNeoloadTest.NeoLoad;
using Neotys.DataExchangeAPI.Client;
using Neotys.DataExchangeAPI.Model;
using Neotys.DesignAPI.Client;
using Neotys.DesignAPI.Model;
using DesignState = Neotys.DesignAPI.Model.Status;
using Ranorex;
using Ranorex.Core.Testing;
using Ranorex.NeoLoad;

namespace NeoloadDesignTest
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	
	[UserCodeCollection]
	public class NeoloadDesignAPIWrapper
	{
		
		private static NeoloadDesignAPIWrapper _instance = null;
		
		private static string DEFAULT_USER_PATH = "UserPath";
		private static string TRANSACTION_TIMER_NAME = "Timer";
		private static string OPT_RANOREX_NEOLOAD_MODE = "nl.ranorex.neoload.mode";

		private static Mode _mode;
		private static string _url;
		private static string _apiKey;

		private static bool   _proxyInUse;
		private static string _proxySettings;
		private static string _proxyOverride;

		private static IDesignAPIClient _client = null;
		private IDataExchangeAPIClient _dataExchangeClient;
		
		private static OpenProjectParamsBuilder   _openProjectPB   = new OpenProjectParamsBuilder();
		private static CloseProjectParamsBuilder  _closeProjectPB  = new CloseProjectParamsBuilder();
		private static CreateProjectParamsBuilder _createProjectPB = new CreateProjectParamsBuilder();
		private static SaveAsProjectParamsBuilder _saveAsProjectPB = new SaveAsProjectParamsBuilder();
		private static IsProjectOpenParamsBuilder _isProjectOpenPB = new IsProjectOpenParamsBuilder();

		private static StartRecordingParamsBuilder _startRecordingPB = new StartRecordingParamsBuilder();
		private static StopRecordingParamsBuilder  _stopRecordingPB  = new StopRecordingParamsBuilder();
		
		private static ContainsUserPathParamsBuilder _containsUserPathPB = new ContainsUserPathParamsBuilder();
		private static UpdateUserPathParamsBuilder   _updateUserPathPB   = new UpdateUserPathParamsBuilder();
		
		private string userPathName;
		private bool pathExists;
		private Context context;
		private TimerBuilder timerBuilder;
		private string transactionName;
		
		public struct NeoloadContextData
		{
			public string hardware;
			public string location;
			public string software;
			public string script;
			public string osFriendlyName;
		}
		
		
		private NeoloadDesignAPIWrapper()
		{
			string globalMode = null;
			try
			{
				globalMode = TestSuite.Current.Parameters[OPT_RANOREX_NEOLOAD_MODE];
			}
			catch (Exception e)
			{
				// Do nothing
			}
			
			if(!String.IsNullOrEmpty(globalMode))
			{
				_mode = (Mode)Enum.Parse(typeof(Mode), globalMode.ToUpper());
			}
			else
			{
				_mode = ModeHelper.getPropertyValue(OPT_RANOREX_NEOLOAD_MODE, Mode.NO_API);
			}
		}
		
		~NeoloadDesignAPIWrapper()
		{
			unsetNeoloadProxy();
		}
		
		private static Context CreateContext(NeoloadContextData ctx)
		{
			ContextBuilder cb = new ContextBuilder();
			cb.Hardware = ctx.hardware;
			cb.Location = ctx.location;
			cb.Software = ctx.software;
			cb.Script = ctx.script;
			cb.Os = ctx.osFriendlyName;

			return cb.build();
		}
		
		public static NeoloadDesignAPIWrapper GetNeoloadDesignTimeWrapper
		{
			get
			{
				if (_instance == null)
					_instance = new NeoloadDesignAPIWrapper();

				return _instance;
			}
		}
		
		private static List<string> BuildArgList(
			string testCase,
			string browser,
			string transaction,
			string param,
			IEnumerable<string> subPath)
		{
			List<string> path = CreateCommonRootPath(testCase, browser, transaction);
			path.AddRange(subPath);
			path.Add(param);

			return path;
		}
		
		private static List<string> CreateCommonRootPath(string testCase, string browser, string transaction)
		{
			List<string> list = new List<string>(); 
			if(!String.IsNullOrEmpty(transaction))
			   {
			   	list.Add(transaction);
			   }
			if(!String.IsNullOrEmpty(testCase))
			   {
			   	list.Add(testCase);
			   }
			return list;
		}
		
		private static Entry BuildEntry(
			NavigationTimingWrapper.NavTiming timing,
			string URL,
			IList<string> pathArgumentList)
		{
			EntryBuilder eb = new EntryBuilder(pathArgumentList);
			eb.Unit = "Milliseconds";
			eb.Value = timing.Value;
			eb.Url = URL;

			return eb.Build();
		}

		
		private bool isProjectOpen()
		{
			var status = _client.GetStatus();
			
			if(status == DesignState.NO_PROJECT)
				return false;
			else
				return true;
		}

		
		private bool isNeoloadReady()
		{
			var status = _client.GetStatus();
			
			if(status == DesignState.READY)
				return true;
			else
				return false;
		}
		
		public void SendNavTiming(IEnumerable<NavigationTimingWrapper.NavTiming> navtiming,
		                          string transactionName,
		                          string url,
		                          string browser,
		                          string testCase)
		{
			if (_mode != Mode.RUNTIME && _mode != Mode.END_USER_EXPERIENCE)
			{
				return;
			}
			CheckDataExchangeIsConnected();
			var entriesToSend = navtiming.Select(nt =>
			                                     BuildEntry(
			                                     	nt,
			                                     	url,
			                                     	BuildArgList(
			                                     		testCase,
			                                     		browser,
			                                     		transactionName,
			                                     		nt.Key,
			                                     		nt.SubPath)))
				.ToList();

			this._dataExchangeClient.AddEntries(entriesToSend);
		}
		
		private void CheckDataExchangeIsConnected()
		{
			if (this._dataExchangeClient == null)
			{
				throw new InvalidOperationException(string.Format("Not connected to NeoLoad data exchange API. This action requires such a connection. Please add a '{0}' module to your test suite that is executed before this action.", "ConnectDataExchangeApi"));
			}
		}
		
		public void ConnectToDesignApi(string designApiUrl, string apiKey)
		{
			if (_mode == Mode.NO_API)
			{
				return;
			}

			_url = designApiUrl;
			_apiKey = apiKey;

			_client = DesignAPIClientFactory.NewClient(_url, _apiKey);
		}
		
		public void ConnectToDataExchangeApi(string dataExchangeApiUrl, string apiKey, NeoloadContextData ctx)
		{
			if (_mode != Mode.RUNTIME && _mode != Mode.END_USER_EXPERIENCE)
			{
				return;
			}
			
			context = CreateContext(ctx);
			if(apiKey == null)
			{
				apiKey = String.Empty;
			}
			this._dataExchangeClient = DataExchangeAPIClientFactory.NewClient(dataExchangeApiUrl, context, apiKey);
		}
		
		private static int getProxyPort()
		{
			int port;
			try
			{
				port = _client.GetRecorderSettings().ProxySettings.Port;
			}
			catch (Exception e)
			{
				throw e;
			}
			return port;
		}

		private static string getProxyHost()
		{
			Uri uri;
			try
			{
				uri = new Uri(_url);
				String domain = uri.Host;
				return domain.StartsWith("www.") ? domain.Substring(4) : domain;
			}
			catch (SystemException ex)
			{
				Console.WriteLine(ex.ToString());
				return "localhost";
			}
		}
		
		
		public void createNewNeoloadProject(string projectName, string directoryPath, bool overwriteExisting)
		{
			if (_mode != Mode.DESIGN)
			{
				return;
			}
			
			this.CheckDesignIsConnected();
			
			if(isProjectOpen())
				throw(new Exception("Error!! Another project is already open. Close it to create a new one!"));
			
			_createProjectPB.name(projectName);
			_createProjectPB.directoryPath(directoryPath);
			_createProjectPB.overwrite(overwriteExisting);
			
			_client.CreateProject(_createProjectPB.Build());
		}

		
		public void openNeoloadProject(string filePath)
		{
			if (_mode == Mode.NO_API)
			{
				return;
			}
			
			this.CheckDesignIsConnected();
			
			if(isProjectOpen())
				throw(new Exception("Error!! Another project is already open. Close it to open this poject:" + filePath));
			
			
			_openProjectPB.filePath(filePath);
			_client.OpenProject(_openProjectPB.Build());
		}
		
		
		public void saveNeoloadProject(TimeSpan timeout, TimeSpan interval)
		{
			if (_mode != Mode.DESIGN)
			{
				return;
			}
			
			this.CheckDesignIsConnected();
			
			var curState = _client.GetStatus();
			switch (curState)
			{
					case DesignState.NO_PROJECT: throw new InvalidOperationException("Failed to save test because no Project is loaded in NeoLoad.");
				default:
					this.WaitForNeoloadState(DesignState.READY, timeout, interval);
					_client.SaveProject();
					break;
			}
		}
		
		
		public void closeNeoloadProject(bool saveProject, bool forceStop)
		{
			if (_mode == Mode.NO_API)
			{
				return;
			}
			
			this.CheckDesignIsConnected();
			
			var status = _client.GetStatus();
			if(status != DesignState.NO_PROJECT)
			{
				_closeProjectPB.save(saveProject);
				_closeProjectPB.forceStop(forceStop);
				_client.CloseProject(_closeProjectPB.Build());
			}
		}
		
		[DllImport("wininet.dll")]
		public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
		public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
		public const int INTERNET_OPTION_REFRESH = 37;
		bool settingsReturn, refreshReturn;
		
		public void startRecording(string userPathName, bool updateUserPath, TimeSpan timeout, TimeSpan interval, string userAgent = "",
		                           bool isWebSocketProtocol = false, bool isHttp2Protocol = false, bool isAdobeRTMPProtocol = false, string addressToExclude = "")
		{
			if (_mode != Mode.DESIGN)
			{
				return;
			}
			
			this.CheckDesignIsConnected();
			
			if(!isNeoloadReady())
				throw(new Exception("Error!! Can't start recording. Neoload is not ready yet."));
			
			this.userPathName = validateUserPathName(userPathName);
			
			_containsUserPathPB.name(userPathName);
			pathExists = _client.ContainsUserPath(_containsUserPathPB.Build());
			if(!updateUserPath && pathExists)
			{
				string msg = String.Format("Error!! UserPath {0} already exists and update path is disabled!", userPathName);
				throw(new Exception(msg));
			}
			
			if (!pathExists)
			{
				_startRecordingPB.virtualUser(userPathName);
			}
			else
			{
				_startRecordingPB.virtualUser(userPathName + "_recording");
			}
			_startRecordingPB.isHTTP2Protocol(isHttp2Protocol);
			_startRecordingPB.isWebSocketProtocol(isWebSocketProtocol);
			_startRecordingPB.isAdobeRTMPProtocol(isAdobeRTMPProtocol);
			_startRecordingPB.userAgent(userAgent);
			
			this.CheckDesignIsConnected();

			var curState = _client.GetStatus();
			switch (curState)
			{
					case DesignState.NO_PROJECT: throw new InvalidOperationException("Failed to start test because no Project is loaded in NeoLoad.");
				default:
					this.WaitForNeoloadState(DesignState.READY, timeout, interval);
					setNeoloadProxy(getProxyHost(), getProxyPort(), addressToExclude);
					_client.StartRecording(_startRecordingPB.Build());
					break;
			}
			
		}


		public void stopRecording(int timeout, bool frameworkParameterSearch = true, bool genericParameterSearch = true,
		                          bool deleteExistingRecording = false, bool includeVariablesInUserpathMerge = true,
		                          int matchingThreshold = 60, bool updateSharedContainers = false)
		{
			if (_mode != Mode.DESIGN)
			{
				return;
			}
			
			this.CheckDesignIsConnected();

			var status = _client.GetStatus();
			if (status != DesignState.BUSY)
				throw (new Exception("Error!! No recording currently running!"));

			if (pathExists)
			{
				_updateUserPathPB.name(this.userPathName);

				_updateUserPathPB.deleteRecording(deleteExistingRecording);
				_updateUserPathPB.includeVariables(includeVariablesInUserpathMerge);
				_updateUserPathPB.matchingThreshold(matchingThreshold);
				_updateUserPathPB.updateSharedContainers(updateSharedContainers);
				_stopRecordingPB.updateParams(_updateUserPathPB.Build());
				Report.Log(ReportLevel.Debug, _updateUserPathPB.Build().ToString());
			}

			_stopRecordingPB.timeout(timeout);
			_stopRecordingPB.frameworkParameterSearch(frameworkParameterSearch);
			_stopRecordingPB.genericParameterSearch(genericParameterSearch);

			Report.Log(ReportLevel.Debug, _stopRecordingPB.Build().ToString());
			try
			{
				_client.StopRecording(_stopRecordingPB.Build());
			}
			catch (Exception e)
			{
				Report.Log(ReportLevel.Debug, e.Message);
			}
		}

		private string validateUserPathName(string userPathName)
		{
			if (userPathName != null && !userPathName.IsEmpty())
			{
				return userPathName;
			}
			return DEFAULT_USER_PATH;
		}
		
		private void CheckDesignIsConnected()
		{
			if (_client == null)
			{
				throw new InvalidOperationException(string.Format(
					"Not connected to NeoLoad design API. NeoLoad actions cannot be used until a connection to a " +
					"NeoLoad server was established. Please add a '{0}' module to your test suite that is executed " +
					"before any NeoLoad action is invoked.", "ConnectToDesignApi"));
			}
		}
		
		private void WaitForNeoloadState(DesignState state, TimeSpan timeout, TimeSpan interval)
		{
			if (!RetryUntil(() => _client.GetStatus() == state, timeout, interval))
			{
				var curState = _client.GetStatus();
				throw new Exception(string.Format("Failed to wait for NeoLoad state '{0}' as it was not reached within the given timeout of '{1}' with a check interval of '{2}'. The last retrieved status was '{3}'.", state, timeout, interval, curState));
			}
		}
		
		private static bool RetryUntil(Func<bool> check, TimeSpan timeout, TimeSpan interval)
		{
			for (var start = System.DateTime.UtcNow; System.DateTime.UtcNow < start + timeout; /* No increment */)
			{
				if (check())
				{
					return true;
				}
				Thread.Sleep(interval);
			}

			return false;
		}
		
		private void setNeoloadProxy(string host, int port, string excluded)
		{
			if (_mode != Mode.DESIGN)
			{
				return;
			}
			
			RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
			
			_proxyInUse = Convert.ToBoolean(registry.GetValue("ProxyEnable"));
			_proxySettings = (string)registry.GetValue("ProxyServer");
			_proxyOverride = (string)registry.GetValue("ProxyOverride");

			registry.SetValue("ProxyEnable", 1);
			registry.SetValue("ProxyServer", String.Format("http={0}:{1};https={0}:{1}",host,port));
			string proxExclusion = host + ":" + getApiPort();
			if(!String.IsNullOrEmpty(excluded))
			{
				excluded = excluded + ";"+ proxExclusion;
			}
			else
			{
				excluded = proxExclusion;
			}
			registry.SetValue("ProxyOverride", excluded);
			settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
			refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
		}
		
		public void unsetNeoloadProxy()
		{
			if (_mode != Mode.DESIGN)
			{
				return;
			}
			
			RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
			
			try {
				if(_proxySettings != null && _proxySettings.Length >0)
				{
					registry.SetValue("ProxyEnable", _proxyInUse ? 1 : 0);
					registry.SetValue("ProxyServer", _proxySettings);
					registry.SetValue("ProxyOverride", _proxyOverride);
				}
				else
				{
					registry.SetValue("ProxyEnable", 0);
					registry.SetValue("ProxyServer", "");
					registry.SetValue("ProxyOverride", "");
				}
				settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
				refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
			}
			catch (Exception e) {
				throw e;
			}
		}
		
		private static int getApiPort()
		{
			Uri uri;
			try
			{
				uri = new Uri(_url);
				return uri.Port;
			}
			catch (SystemException ex)
			{
				Console.WriteLine(ex.ToString());
				return 7400;
			}
		}
		
		
		[UserCodeMethod]
		public void StartTransaction(string name)
		{
			if(Mode.DESIGN == _mode)
			{
				this.CheckDesignIsConnected();
				var status = _client.GetStatus();
				if(status != DesignState.BUSY)
					throw(new Exception("Error!! No recording currently running!"));
				_client.SetContainer(new SetContainerParams(name));
			}
			else if(Mode.END_USER_EXPERIENCE == _mode || Mode.RUNTIME == _mode)
			{
				CheckDataExchangeIsConnected();
				if (transactionName != null)
				{
					StopTransaction();
				}
				transactionName = name;
				HandleTimer();
				IList<string> timerPath = NewPath(transactionName);
				timerPath.Add(TRANSACTION_TIMER_NAME);
				timerBuilder = TimerBuilder.Start(timerPath);
			}
		}
		
		public void StopTransaction()
		{
			if(Mode.END_USER_EXPERIENCE == _mode  || Mode.RUNTIME == _mode)
			{
				CheckDataExchangeIsConnected();
				HandleTimer();
				transactionName = null;
				timerBuilder = null;
			}
		}
		
		private void HandleTimer()
		{
			TimerBuilder current = timerBuilder;
			if(current != null && this._dataExchangeClient != null)
			{
				try
				{
					this._dataExchangeClient.AddEntry(current.Stop());
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
		
		private IList<string> NewPath(string transactionName)
		{
			IList<string> path = new List<string>();
			
			path.Add(context.Script);
			path.Add(TimerBuilder.TimersName);
			
			if(this.userPathName != null)
			{
				path.Add(this.userPathName);
			}
			if(!String.IsNullOrEmpty(transactionName))
			{
				path.Add(transactionName);
			}
			
			return path;
		}


	}
}
