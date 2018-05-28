///////////////////////////////////////////////////////////////////////////////////////////////////
//
// This file is part of the  R A N O R E X  Project.
// http://www.ranorex.com
//
///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using MyNeoloadTest.NeoLoad;
using MyNeoloadTest.NeoLoad.config;
using Neotys.CommonAPI.Error;
using Neotys.RuntimeAPI.Client;
using Neotys.RuntimeAPI.Model;
using NtState = Neotys.RuntimeAPI.Model.Status;
using Ranorex.Core.Testing;

namespace Ranorex.NeoLoad
{
	internal class NeoloadApi : INeoloadApi
	{
		public static INeoloadApi Instance { get; private set; }
		
		private static string OPT_RANOREX_NEOLOAD_MODE = "nl.ranorex.neoload.mode";
		private static Mode _mode;
		private IRuntimeAPIClient runtimeClient;
		private ParamBuilderProvider paramBuilderProvider;

		static NeoloadApi()
		{
			Instance = new NeoloadApi();
		}

		private NeoloadApi()
		{
			this.paramBuilderProvider = new ParamBuilderProvider();
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

		public void ConnectToRuntimeApi(string runtimeApiUrl, string apiKey)
		{
			if (_mode != Mode.RUNTIME)
			{
				return;
			}
			
			if(apiKey == null)
			{
				apiKey = String.Empty;
			}
			this.runtimeClient = RuntimeAPIClientFactory.NewClient(runtimeApiUrl, apiKey);
		}

		public void StartNeoLoadTest(string scenario, TimeSpan timeout, TimeSpan interval)
		{
			if (_mode != Mode.RUNTIME)
			{
				return;
			}
			this.CheckRuntimeIsConnected();

			var curState = this.runtimeClient.getStatus();
			switch (curState)
			{
					case NtState.NO_PROJECT: throw new InvalidOperationException("Failed to start test because no Project is loaded in NeoLoad.");
					case NtState.TEST_RUNNING: throw new InvalidOperationException("A Neotys test is already running. Cannot start new test run.");
				default:
					this.WaitForNeoloadRuntimeState(NtState.READY, timeout, interval);
					this.runtimeClient.StartTest(new StartTestParamsBuilder(scenario).Build());
					this.WaitForNeoloadRuntimeState(NtState.TEST_RUNNING, timeout, interval);
					break;
			}
		}

		public void StopNeoLoadTest(TimeSpan timeout, TimeSpan interval, Boolean forceStop)
		{
			if (_mode != Mode.RUNTIME)
			{
				return;
			}
			
			this.CheckRuntimeIsConnected();

			var curState = this.runtimeClient.getStatus();
			switch (curState)
			{
					case NtState.TEST_STOPPING: Report.Warn("Cannot stop NeoLoad test because it is already stopping."); break;
				case NtState.TEST_RUNNING:
					StopTestParamsBuilder builder = new StopTestParamsBuilder();
					builder.ForceStop = forceStop;
					this.runtimeClient.StopTest(builder.Build());
					this.WaitForNeoloadRuntimeState(NtState.READY, timeout, interval);
					break;
				default:
					var s = this.runtimeClient.getStatus();
					throw new Exception(string.Format("Cannot stop NeoLoad test because no test is running. The current status of the NeoLoad system is '{0}'.", s));
			}
		}

		private void WaitForNeoloadRuntimeState(NtState state, TimeSpan timeout, TimeSpan interval)
		{
			if (!RetryUntil(() => this.runtimeClient.getStatus() == state, timeout, interval))
			{
				var curState = this.runtimeClient.getStatus();
				throw new Exception(string.Format("Failed to wait for NeoLoad state '{0}' as it was not reached within the given timeout of '{1}' with a check interval of '{2}'. The last retrieved status was '{3}'.", state, timeout, interval, curState));
			}
		}

		private static bool RetryUntil(Func<bool> check, TimeSpan timeout, TimeSpan interval)
		{
			for (var start = System.DateTime.UtcNow; System.DateTime.UtcNow < start + timeout; /* No increment */)
			{
				Thread.Sleep(interval);
				if (check())
				{
					return true;
				}
			}

			return false;
		}

		public void AddVirtualUsers(string population, int ammount)
		{
			if (_mode != Mode.RUNTIME)
			{
				return;
			}
			this.CheckRuntimeIsConnected();

			if (runtimeClient.getStatus() != NtState.TEST_RUNNING)
			{
				throw new Exception("Cannot add/remove virtual users when no NeoLoad test is running.");
			}

			this.runtimeClient.AddVirtualUsers(new AddVirtualUsersParamsBuilder(population, ammount).Build());
		}

		public void RemoveVirtualUsers(string population, int ammount)
		{
			if (_mode != Mode.RUNTIME)
			{
				return;
			}
			this.CheckRuntimeIsConnected();

			if (runtimeClient.getStatus() != NtState.TEST_RUNNING)
			{
				throw new Exception("Cannot add/remove virtual users when no NeoLoad test is running.");
			}

			this.runtimeClient.StopVirtualUsers(new StopVirtualUsersParamsBuilder(population, ammount).Build());
		}

		private void CheckRuntimeIsConnected()
		{
			if (this.runtimeClient == null)
			{
				throw new InvalidOperationException(string.Format(
					"Not connected to NeoLoad runtime API. NeoLoad actions cannot be used until a connection to a " +
					"NeoLoad server was established. Please add a '{0}' module to your test suite that is executed " +
					"before any NeoLoad action is invoked.", "ConnectToRuntimeApi"));
			}
		}

	}
}
