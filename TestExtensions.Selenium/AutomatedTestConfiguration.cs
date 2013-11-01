using System;
using System.Configuration;
using System.Reflection;

namespace TestExtensions.Selenium
{
	public static class AutomatedTestConfiguration
	{
		static AutomatedTestConfiguration()
		{
			var driverClassName = GetSettingValue("Driver");
			var assemblyName = GetSettingValue("Assembly");
			var assembly = Assembly.Load(assemblyName);
			BrowserMake = (BrowserMake)Enum.Parse(typeof(BrowserMake), GetSettingValue("BrowserMake"), true);
			DriverType = assembly.GetType(driverClassName);
		}

		public static string BaseUrl
		{
			get { return "https://" + EnvironmentBaseUrl + "/" + CurrentCulture; }
		}

		public static int BrowserHeight
		{
			get
			{
				var result = 0;
				var browserSize = GetSettingValue("BrowserSize");
				var split = browserSize.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

				if (split.Length > 1)
				{
					int.TryParse(split[1], out result);
				}
				return result;
			}
		}

		public static BrowserMake BrowserMake { get; private set; }

		public static int BrowserWidth
		{
			get
			{
				var result = 0;
				var browserSize = GetSettingValue("BrowserSize");
				var split = browserSize.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

				if (split.Length > 1)
				{
					int.TryParse(split[0], out result);
				}
				return result;
			}
		}

		public static string CurrentCulture
		{
			get { return GetSettingValue("CurrentCulture"); }
		}

		public static string DefaultLanguage
		{
			get { return GetSettingValue("DefaultLanguage"); }
		}

		public static TimeSpan DefaultPageTimeOut
		{
			get { return TimeSpan.FromSeconds(int.Parse(GetSettingValue("DefaultPageTimeOut"))); }
		}

		public static string DefaultUserFriend
		{
			get { return GetSettingValue("DefaultUserFriend"); }
		}

		public static string DefaultUsername
		{
			get { return GetSettingValue("DefaultUsername"); }
		}

		public static Type DriverType { get; private set; }

		public static string EnvironmentBackOfficeBaseUrl
		{
			get { return "https://" + GetSettingValue("EnvironmentBackOfficeBaseUrl"); }
		}

		public static string EnvironmentBaseUrl
		{
			get { return GetSettingValue("EnvironmentBaseUrl"); }
		}

		public static bool MaximiseBrowser
		{
			get
			{
				var browserSize = GetSettingValue("BrowserSize");
				return browserSize.ToLowerInvariant() == "max";
			}
		}

		public static bool NewUser
		{
			get { return bool.Parse(GetSettingValue("DefaultUserNew")); }
		}

		public static bool UseConfigurationUser
		{
			get { return bool.Parse(GetSettingValue("UseConfigurationUser")); }
		}

		public static string Username
		{
			get { return GetSettingValue("username"); }
		}

		private static string GetSettingValue(string key)
		{
			var strings	 = ConfigurationManager.AppSettings.GetValues(key);
			return strings != null ? strings[0] : null;
		}
	}
}