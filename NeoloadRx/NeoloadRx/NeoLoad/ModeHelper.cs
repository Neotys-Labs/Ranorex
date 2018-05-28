/*
 * Created by Ranorex
 * User: mlasram
 * Date: 28/05/2018
 * Time: 14:06
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Text.RegularExpressions;

namespace MyNeoloadTest.NeoLoad
{
	
	public class ModeHelper
	{
		
		public static Mode getPropertyValue(string key, Mode defaultValue)
		{
			string envValue = Environment.GetEnvironmentVariable(key);
			if (envValue != null)
			{
				return (Mode)Enum.Parse(typeof(Mode), envValue);
			}

			string pattern = "-D" + Regex.Escape(key) + "=" + "(.+)";
			foreach (string arg in Environment.GetCommandLineArgs())
			{
				MatchCollection matchCollection = Regex.Matches(arg, pattern);
				if (matchCollection.Count > 0)
				{
					return (Mode)Enum.Parse(typeof(Mode), matchCollection[0].Groups[1].Value.ToUpper());
				}
			}
			return defaultValue;
		}
	}
	public enum Mode
	{
		DESIGN, END_USER_EXPERIENCE, NO_API, RUNTIME
	}
}
