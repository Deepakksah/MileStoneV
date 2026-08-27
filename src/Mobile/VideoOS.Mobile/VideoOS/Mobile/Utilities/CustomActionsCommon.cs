using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Xml;
using Microsoft.Deployment.WindowsInstaller;
using VideoOS.Mobile.Configuration;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Utilities;

public class CustomActionsCommon
{
	private enum SendMessageTimeoutFlags : uint
	{
		SMTO_NORMAL = 0u,
		SMTO_BLOCK = 1u,
		SMTO_ABORTIFHUNG = 2u,
		SMTO_NOTIMEOUTIFNOTHUNG = 8u
	}

	public const string CallerWizardName = "{74CDB09C-BDEF-4FA6-ABF4-088DF4546501}";

	public const string UseAllParamsName = "{3B447035-BD97-4C86-8CF1-994B41A12D22}";

	public const string UseSomeParamsName = "{58E081CE-AE02-4062-91F3-902B99BC673C}";

	public static Guid CallerWizardId = new Guid("{74CDB09C-BDEF-4FA6-ABF4-088DF4546501}");

	public static Guid UseAllParamsCallerId = new Guid("{3B447035-BD97-4C86-8CF1-994B41A12D22}");

	public static Guid UseSomeParamsCallerId = new Guid("{58E081CE-AE02-4062-91F3-902B99BC673C}");

	public const int HWND_BROADCAST = 65535;

	private const uint TRAY_MESSAGE_TIMEOUT = 500u;

	private const int TRAY_PROCESS_KILL_TIMEOUT = 3000;

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern uint RegisterWindowMessage(string message);

	public static bool CheckServerUriString(Session session)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (!session["URI"].StartsWith("http://") && !session["URI"].StartsWith("https://"))
		{
			session["URI"] = "http://" + session["URI"];
		}
		string text = session["URI"].SanitizeUri();
		session.Log("Using URI=" + text);
		Record val = null;
		Uri result = null;
		bool flag = false;
		if (!string.IsNullOrEmpty(text) && Uri.TryCreate(text, UriKind.Absolute, out result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
		{
			flag = true;
		}
		if (!flag)
		{
			val = new Record(1);
			val.SetString(0, session["URIWRONGFORMAT"]);
			session.Message((InstallMessage)16777232, val);
			session["CheckServerUri"] = "1";
			return false;
		}
		session["SERVERHOSTNAME"] = result.Host;
		session["SERVERPORT"] = result.Port.ToString();
		session["CheckServerUri"] = "0";
		session.Log("ServerHostname now set to:" + session["SERVERHOSTNAME"]);
		return true;
	}

	public static bool InsertServerIdInConfig(Session session, InstallerConfiguration isntallerConfiguration, string exeFile)
	{
		bool flag = session.CustomActionData.ContainsKey("Uninstall") && session.CustomActionData["Uninstall"] == "true";
		bool mode = session.GetMode((InstallRunMode)16);
		try
		{
			if (mode && !flag)
			{
				ConfigFile configFile = new ConfigFile(Path.Combine(new SessionVariables(session).Path, exeFile));
				string serviceId = isntallerConfiguration.ServerId ?? Guid.NewGuid().ToString();
				configFile.SetValue(delegate(XmlDocument doc)
				{
					doc.SelectSingleNode("configuration/ServerSetings/Identification").InsertКеyValue("ServiceId", serviceId);
				});
			}
		}
		catch (Exception ex)
		{
			session.Log("InsertServerIdInConfig(), Error (un)registering Mobile Server to XProtect: {0}", new object[1] { ex.Message });
			return false;
		}
		return true;
	}

	public static string GetPasswordBase64(string mosServicePassword)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Record val = null;
		string result = "";
		try
		{
			string text = Convert.ToBase64String(Encoding.Unicode.GetBytes(mosServicePassword));
			val = new Record(1);
			val.SetString(1, text);
			result = (string)val[1];
			return result;
		}
		catch (Exception)
		{
			return result;
		}
		finally
		{
			if (val != null)
			{
				((InstallerHandle)val).Close();
			}
		}
	}

	public static bool GetCurrentUser(Session session)
	{
		try
		{
			session.Log("GetCurrentUser Started");
			string name = WindowsIdentity.GetCurrent().Name;
			if (!string.IsNullOrEmpty(name))
			{
				session["CURRENT_USER"] = name;
			}
			session.Log("GetCurrentUser UserName = {0}", new object[1] { name });
		}
		catch (Exception ex)
		{
			session.Log("GetCurrentUser Failed, Exception: {0}", new object[1] { ex.Message });
			return false;
		}
		return true;
	}

	public static bool DeleteFiles(Session session, string keyPath, RegistryHelperBase instance)
	{
		return DeleteDirectory(session, instance.Read(keyPath));
	}

	public static bool DeleteDirectory(Session session, string path)
	{
		try
		{
			Directory.Delete(path, recursive: true);
			return true;
		}
		catch (Exception ex)
		{
			session.Log("DeleteFiles of " + path + " failed with message: " + ex.Message);
			return false;
		}
	}

	public static bool TerminateTrayController(Session session, string wmAsktrayquit)
	{
		if (!TerminateTrayGracefully(wmAsktrayquit))
		{
			session.Log("TerminateTrayController(), Failed to terminate Tray Controller. Timeout or wrong message result. Force close will be attempted.");
		}
		string trayProcessName = session["TRAYPROCNAME"].Sanitized(".");
		if (!ForceCloseTrayIfNeeded(session, trayProcessName))
		{
			session.Log("TerminateTrayController(), Failed to force close Tray Controller. Cannot continue uninstall.");
			return false;
		}
		return true;
	}

	public static void SetRegistryAccessRights(string serviceUsername, RegistryHelperBase instance, List<string> errorLog)
	{
		if (!instance.SetAccountRights(serviceUsername))
		{
			errorLog.Add("Failed to grant access to registry for NetworkService account");
		}
	}

	public static ActionResult InsertInstallationFolderInRegistry(Session session, RegistryHelperBase instance)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			SetInRegistry(instance, "InstallationFolder", Path.Combine(session.CustomActionData["ProgramFolder"], session.CustomActionData["APPNAME"]), session);
			ClearOldRegistryEntry(instance, "SurveillanceUsername");
			ClearOldRegistryEntry(instance, "SurveillancePassword");
			ClearOldRegistryEntry(instance, "LogFolder");
			ClearOldRegistryEntry(instance, "OSMADDRESSES");
			ClearOldRegistryEntry(instance, "PORTHTTP");
			ClearOldRegistryEntry(instance, "PORTHTTPSECURED");
			ClearOldRegistryEntry(instance, "PORTHTTPWCF");
			ClearOldRegistryEntry(instance, "SERVERID");
			ClearOldRegistryEntry(instance, "SERVERIDS");
			ClearOldRegistryEntry(instance, "SERVERIDT");
			ClearOldRegistryEntry(instance, "SURVEILLANCENAME");
			ClearOldRegistryEntry(instance, "SURVEILLANCEPORT");
			ClearOldRegistryEntry(instance, "ExportFolder");
			return (ActionResult)0;
		}
		catch (Exception ex)
		{
			session.Log("InsertInstallationFolderInRegistry failed:" + ex.Message);
			return (ActionResult)1603;
		}
	}

	public static void InsertCertificatePasswordInConfig(Session session, string password, string exeFile)
	{
		new ConfigFile(Path.Combine(new SessionVariables(session).Path, exeFile + ".config")).SetValue(delegate(XmlDocument doc)
		{
			doc.SelectSingleNode("configuration/ServerSetings/Identification").InsertКеyValue("kluch", password);
		});
	}

	public static ActionResult SetFilesAccessRights(string serviceUsername, string keyPath, RegistryHelperBase instance, List<string> errorLog)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DirectoryHelper.AddDirectorySecurity(instance.Read(keyPath), serviceUsername);
			return (ActionResult)0;
		}
		catch (Exception ex)
		{
			errorLog.Add("Grant access to " + keyPath + " for Network Service account failed with message: " + ex.Message);
			return (ActionResult)1603;
		}
	}

	public static void SetInRegistry(RegistryHelperBase instance, string entryName, object entryValue, Session session)
	{
		if (!instance.Write(entryName, entryValue))
		{
			session.Log($"InternalConfig.SetInRegistry() - Failed to write registry {entryName}");
		}
	}

	public static void ClearOldRegistryEntry(RegistryHelperBase instance, string registryName)
	{
		if (instance.HasValue(registryName, (string data) => !string.IsNullOrEmpty(data)))
		{
			instance.DeleteKey(registryName);
		}
	}

	private static bool ForceCloseTrayIfNeeded(Session session, string trayProcessName)
	{
		bool result = true;
		if (string.Empty == trayProcessName)
		{
			return true;
		}
		try
		{
			Process[] processesByName = Process.GetProcessesByName(trayProcessName);
			if (processesByName.Length < 1)
			{
				session.Log("ForceCloseTrayIfNeeded(), No tray processes found.");
				return true;
			}
			Process[] array = processesByName;
			foreach (Process process in array)
			{
				session.Log("ForceCloseTrayIfNeeded(), Killing process " + process.ProcessName);
				process.Kill();
				if (!process.WaitForExit(3000))
				{
					result = false;
					break;
				}
			}
		}
		catch (Win32Exception ex)
		{
			session.Log("ForceCloseTrayIfNeeded(), Could not terminate process, may be closing or already closed. Message: " + ex.Message);
		}
		catch (Exception ex2)
		{
			session.Log("ForceCloseTrayIfNeeded(), Error terminating process. Message: " + ex2.Message);
			result = false;
		}
		return result;
	}

	private static bool TerminateTrayGracefully(string wmAsktrayquit)
	{
		IntPtr result;
		return SendMessageTimeout((IntPtr)65535, RegisterWindowMessage(wmAsktrayquit), IntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_BLOCK, 500u, out result) != IntPtr.Zero;
	}

	public static void DecryptSessionPassword(Session session)
	{
		if (session["PASSWORD"] != null && !session["PASSWORD"].Equals(string.Empty))
		{
			session["PASSWORD"] = DecryptPassword(session["PASSWORD"], unicodePassword: true);
		}
	}

	public static bool CallerIsWizardPage(Session session)
	{
		bool result = false;
		try
		{
			result = CallerWizardId.Equals(new Guid(session["CALLER"]));
		}
		catch
		{
		}
		return result;
	}

	public static bool CallerIsWizardPage(string caller)
	{
		bool result = false;
		try
		{
			result = CallerWizardId.Equals(new Guid(caller));
		}
		catch
		{
		}
		return result;
	}

	private static string DecryptPassword(string encryptedPass, bool unicodePassword)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(encryptedPass))
		{
			try
			{
				byte[] optionalEntropy = new byte[8] { 20, 70, 139, 161, 137, 60, 220, 152 };
				byte[] bytes = ProtectedData.Unprotect(Convert.FromBase64String(encryptedPass), optionalEntropy, DataProtectionScope.CurrentUser);
				result = ((!unicodePassword) ? Encoding.ASCII.GetString(bytes) : Encoding.Unicode.GetString(bytes));
			}
			catch (FormatException)
			{
				return "";
			}
		}
		return result;
	}

	private static void DumpActionData(Session session)
	{
		foreach (string key in session.CustomActionData.Keys)
		{
			session.Log("CustomActionData " + key + " -- " + session.CustomActionData[key]);
		}
	}

	public static void LoadInstallerConfigFromExeConfig(InstallerConfiguration installerConfig, string exeConfigFullPath)
	{
		XmlDocument value = new ConfigFile(exeConfigFullPath).GetValue((XmlDocument d) => d);
		installerConfig.LoggingFilePath = value.SelectSingleNode("configuration/ServerSetings/Logging")?.GetConfigEntry("LoggingFilePath") ?? installerConfig.LoggingFilePath;
		installerConfig.OsmAddresses = new List<string>();
		foreach (XmlNode item in value.SelectNodes("configuration/GisMaps/OsmAddresses/add"))
		{
			foreach (XmlAttribute attribute in item.Attributes)
			{
				if (attribute.Name == "address")
				{
					installerConfig.OsmAddresses.Add(attribute.Value);
				}
			}
		}
		installerConfig.AccessControlAllowOrigins = value.SelectSingleNode("configuration/ServerSetings/HttpHeaders")?.GetConfigEntry("Access-Control-Allow-Origin") ?? installerConfig.AccessControlAllowOrigins;
		XmlNode xmlNode = value.SelectSingleNode("configuration/ServerSetings/Identification");
		installerConfig.CertificationPassword = xmlNode?.GetConfigEntry("kluch") ?? "";
		installerConfig.ServerId = xmlNode?.GetConfigEntry("ServiceId") ?? installerConfig.ServerId;
		installerConfig.ExportsFolder = value.SelectSingleNode("configuration/ExportSettings/Location")?.GetConfigEntry("ExportsLocation") ?? installerConfig.ExportsFolder;
		xmlNode = value.SelectSingleNode("configuration/ManagementServer/Address");
		string configEntry = xmlNode.GetConfigEntry("Scheme");
		configEntry = (string.IsNullOrWhiteSpace(configEntry) ? "http" : configEntry);
		string input = configEntry + "://" + xmlNode.GetConfigEntry("Ip") + ":" + xmlNode.GetConfigEntry("Port");
		installerConfig.Uri = input.SanitizeUri() ?? installerConfig.Uri;
		installerConfig.AbsServiceHost = value.SelectSingleNode("configuration/OnlineServices/AbsService")?.GetConfigEntry("Hostname") ?? installerConfig.AbsServiceHost;
		installerConfig.OnlineNotificationServiceHost = value.SelectSingleNode("configuration/OnlineServices/OnlineNotificationService")?.GetConfigEntry("Hostname") ?? installerConfig.OnlineNotificationServiceHost;
	}
}
