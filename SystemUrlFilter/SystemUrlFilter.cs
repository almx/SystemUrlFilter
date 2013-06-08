using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SystemUrlFilter
{
	public static class SystemUrlFilter
	{
		public static void Filter(string[] args)
		{
			string filterFilePath = Path.Combine(Application.StartupPath, "Filters.txt");

			// Hack for unit testing
			if (!File.Exists(filterFilePath))
				filterFilePath = Path.Combine(Assembly.GetExecutingAssembly().Location.Replace("SystemUrlFilter.exe", ""), "Filters.txt");

			if (!File.Exists(filterFilePath))
				throw new Exception(String.Format("Filters.txt file not found in location '{0}'", filterFilePath));

			string url = GetUrlFromArgs(args);

			if (ConfigurationManager.AppSettings["Logging"] == "true")
				Log(args);

			List<string> filters = new List<string>(File.ReadAllLines(filterFilePath));

			PerformFiltering(url, filters);
		}

		private static void PerformFiltering(string url, List<string> filters)
		{
			foreach (string filter in filters)
			{
				string filterWithoutWildcards = GetFilterWithoutWildcards(filter);

				if (Regex.IsMatch(filter, @"^\*.+\*$") && !url.Contains(filterWithoutWildcards)
					|| Regex.IsMatch(filter, @"^\*.+[^\*]$") && !url.EndsWith(filterWithoutWildcards) && !url.StartsWith(filterWithoutWildcards)
					|| Regex.IsMatch(filter, @"^[^\*].+\*$") && !url.EndsWith(filterWithoutWildcards) && !url.StartsWith(filterWithoutWildcards)
					|| Regex.IsMatch(filter, @"^[^\*].+[^\*]$") && filter != url)
				{
					StartBrowser(url);
				}
			}
		}

		private static void StartBrowser(string url)
		{
			Process proc = new Process();
			proc.StartInfo.FileName = ConfigurationManager.AppSettings["BrowserExecutablePath"];
			proc.StartInfo.Arguments = String.Format("{0}\"{1}\"", ConfigurationManager.AppSettings["BrowserArgumentsBeforeUrl"], url);

			proc.Start();
		}

		public static string GetFilterWithoutWildcards(string filter)
		{
			if (filter.StartsWith("*"))
				filter = filter.Substring(1);

			if (filter.EndsWith("*"))
				filter = filter.Substring(0, filter.LastIndexOf("*"));

			return filter;
		}

		private static string GetUrlFromArgs(string[] args)
		{
			if (args.Length != 1 || args[0].Length == 0)
				throw new Exception("Supply one URL as argument string");
			else
				return args[0];
		}

		private static void Log(string[] args)
		{
			string loggingDirectory = Application.StartupPath;

			if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["LoggingDirectoryOverride"]) && Directory.Exists(ConfigurationManager.AppSettings["LoggingDirectoryOverride"]))
				loggingDirectory = ConfigurationManager.AppSettings["LoggingDirectoryOverride"];

			using (TextWriter tw = new StreamWriter(Path.Combine(loggingDirectory, "SystemUrlFilter.log"), true))
			{
				tw.WriteLine(String.Format("{0}: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), args[0].ToString()));
			}
		}
	}
}
