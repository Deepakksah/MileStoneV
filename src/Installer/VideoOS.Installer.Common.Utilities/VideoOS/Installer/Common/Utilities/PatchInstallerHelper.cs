using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32;

namespace VideoOS.Installer.Common.Utilities;

public static class PatchInstallerHelper
{
	public enum ActionTargetName
	{
		BeforeUpdateAction,
		AfterUpdateAction,
		UninstallUpdatesAction
	}

	public enum ProductName
	{
		NotSelected,
		ManagementServer,
		RecordingServer,
		ManagementClient,
		EventServer,
		MobileServer,
		SmartClient
	}

	private const string PatchInstallerModuleLocation = "PI_MODULE_LOCATION";

	private const string PatchInstallerCustomActionClassName = "PI_MODULE_CUSTOM_ACTION_CLASS_NAME";

	private const string PatchInstallerBeforeActionMethodName = "PI_MODULE_BEFORE_UPDATE_ACTION_NAME";

	private const string PatchInstallerAfterActionMethodName = "PI_MODULE_AFTER_UPDATE_ACTION_NAME";

	private const string PatchInstallerUninstallUpdatesMethodName = "PI_MODULE_UNINSTALL_METHOD_NAME";

	public static ActionResult CallCustomAction(Session session, ActionTargetName actionTarget, ProductName productName = ProductName.NotSelected, [CallerMemberName] string methodName = null)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if (session == null)
		{
			throw new ArgumentNullException("session");
		}
		return (ActionResult)(actionTarget switch
		{
			ActionTargetName.BeforeUpdateAction => CallCustomActionFromPatchInstaller(session, methodName, "PI_MODULE_BEFORE_UPDATE_ACTION_NAME"), 
			ActionTargetName.AfterUpdateAction => CallCustomActionFromPatchInstaller(session, methodName, "PI_MODULE_AFTER_UPDATE_ACTION_NAME"), 
			ActionTargetName.UninstallUpdatesAction => UninstallUpdates(session, productName), 
			_ => 1626, 
		});
	}

	private static ActionResult CallCustomActionFromPatchInstaller(Session session, string methodName, string actionMethodNameKey)
	{
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		string text = FormattableString.Invariant($"{DateTime.Now:O}:{methodName}:{actionMethodNameKey}:START");
		session.Log(text);
		try
		{
			string text2 = session["PI_MODULE_LOCATION"];
			string customActionClassName = session["PI_MODULE_CUSTOM_ACTION_CLASS_NAME"];
			string text3 = session[actionMethodNameKey];
			session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{methodName}:{actionMethodNameKey}: Dll={text2}, Class={customActionClassName}, ActionMethod={text3}"));
			if (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(customActionClassName) || string.IsNullOrEmpty(text3) || !File.Exists(text2))
			{
				return (ActionResult)1626;
			}
			Type type = Assembly.LoadFrom(text2).GetExportedTypes().FirstOrDefault((Type t) => t.Name.IndexOf(customActionClassName, StringComparison.OrdinalIgnoreCase) != -1);
			if (type != null)
			{
				object obj = Activator.CreateInstance(type);
				MethodInfo method = type.GetMethod(text3);
				session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{methodName}:{actionMethodNameKey}: Calling PatchInstallerModule:{customActionClassName}.{text3}:START"));
				object[] parameters = (object[])(object)new Session[1] { session };
				method.Invoke(obj, parameters);
				session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{methodName}:{actionMethodNameKey}: Calling PatchInstallerModule:{customActionClassName}.{text3}:END"));
			}
			return (ActionResult)0;
		}
		catch (Exception ex)
		{
			session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{methodName}:{actionMethodNameKey}:ERROR:{ex}"));
			return (ActionResult)1603;
		}
		finally
		{
			session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{methodName}:{actionMethodNameKey}:END"));
		}
	}

	private static ActionResult UninstallUpdates(Session session, ProductName productName)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		string text = "UninstallUpdates";
		session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{text}:Begin"));
		string text2 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
		try
		{
			session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{text}:Getting Product Updates"));
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
			{
				using RegistryKey registryKey2 = registryKey.OpenSubKey(text2);
				if (registryKey2 == null)
				{
					session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{text}:Unable to open [{text2}]"));
					return (ActionResult)1603;
				}
				ProcessUninstallUpdatesFromRegistryKey(session, registryKey2, productName);
			}
			using RegistryKey registryKey3 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			using RegistryKey registryKey4 = registryKey3.OpenSubKey(text2);
			if (registryKey4 == null)
			{
				session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{text}:Unable to open [{text2}]"));
				return (ActionResult)1603;
			}
			ProcessUninstallUpdatesFromRegistryKey(session, registryKey4, productName);
		}
		catch (Exception ex)
		{
			session.Log(ex.ToString());
		}
		finally
		{
			session.Log(FormattableString.Invariant($"{DateTime.Now:O}:{text}:End"));
		}
		return (ActionResult)0;
	}

	private static bool ProcessUninstallUpdatesFromRegistryKey(Session session, RegistryKey uninstallKey, ProductName productName)
	{
		string text = $".Patch.{productName}";
		session.Log(FormattableString.Invariant(FormattableStringFactory.Create("{0:O}:{1}:Lookup Details: TagKey=[{2}]", DateTime.Now, "ProcessUninstallUpdatesFromRegistryKey", text)));
		string[] subKeyNames = uninstallKey.GetSubKeyNames();
		foreach (string name in subKeyNames)
		{
			using RegistryKey registryKey = uninstallKey.OpenSubKey(name);
			if (registryKey == null)
			{
				continue;
			}
			string[] valueNames = registryKey.GetValueNames();
			foreach (string text2 in valueNames)
			{
				if (text2.IndexOf("BundleTag", StringComparison.OrdinalIgnoreCase) == -1)
				{
					continue;
				}
				session.Log(FormattableString.Invariant(FormattableStringFactory.Create("{0:O}:{1}:Discovered Bootstrapper Bundle Entry: [{2}]", DateTime.Now, "ProcessUninstallUpdatesFromRegistryKey", registryKey.Name)));
				object value = registryKey.GetValue(text2);
				if (value == null || value.ToString().IndexOf(text, StringComparison.OrdinalIgnoreCase) == -1)
				{
					continue;
				}
				session.Log(FormattableString.Invariant(FormattableStringFactory.Create("{0:O}:{1}:Discovered an update for {2} in [{3}] view", DateTime.Now, "ProcessUninstallUpdatesFromRegistryKey", productName, uninstallKey.View)));
				string text3 = registryKey.GetValue("BundleCachePath").ToString();
				try
				{
					session.Log(FormattableString.Invariant(FormattableStringFactory.Create("{0:O}:{1}:Trying to uninstall update: [{2}]", DateTime.Now, "ProcessUninstallUpdatesFromRegistryKey", text3)));
					if (File.Exists(text3))
					{
						Process.Start(new ProcessStartInfo(text3, "/uninstall /quiet")
						{
							UseShellExecute = false
						})?.WaitForExit();
						continue;
					}
					session.Log(FormattableString.Invariant(FormattableStringFactory.Create("{0:O}:{1}:File not found: [{2}]", DateTime.Now, "ProcessUninstallUpdatesFromRegistryKey", text3)));
				}
				catch (Exception ex)
				{
					session.Log(ex.ToString());
					return false;
				}
			}
		}
		return true;
	}
}
