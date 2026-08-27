using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Security;
using System.ServiceProcess;
using System.Text;
using Microsoft.Win32;
using VideoOS.Server.Service.Properties;
using VideoOS.Utilities.Service;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Server.Service;

[RunInstaller(true)]
public class ProjectInstaller : Installer
{
	private ServiceProcessInstaller serviceProcessInstaller1;

	private ServiceInstallerEx serviceInstaller1;

	private Container components;

	public ProjectInstaller()
	{
		InitializeComponent();
		serviceInstaller1.ServiceName = OemResources.Instance.Server.ServiceName;
		serviceInstaller1.Description = Resources.ServerServiceDescription;
		serviceInstaller1.StartType = ServiceStartMode.Disabled;
		serviceInstaller1.StartOnInstall = false;
		serviceInstaller1.FailCountResetTime = 600u;
		serviceInstaller1.FailureActions.Add(new FailureAction(RecoverAction.Restart, 60000));
		serviceInstaller1.FailureActions.Add(new FailureAction(RecoverAction.Restart, 60000));
		serviceInstaller1.FailureActions.Add(new FailureAction(RecoverAction.None, 60000));
		if (!EventLog.SourceExists(OemResources.Instance.Server.ServiceName))
		{
			EventLog.CreateEventSource(OemResources.Instance.Server.ServiceName, "Application");
		}
		base.BeforeInstall += Installer_OnBeforeInstall;
		base.BeforeUninstall += ProjectInstaller_BeforeUninstall;
		base.AfterInstall += delegate
		{
			TurnOnAllowServiceToInteractWithDesktop();
		};
	}

	private void TurnOnAllowServiceToInteractWithDesktop()
	{
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey($"SYSTEM\\CurrentControlSet\\Services\\{serviceInstaller1.ServiceName}", writable: true);
		if (registryKey != null)
		{
			object value = registryKey.GetValue("Type");
			if (value != null)
			{
				registryKey.SetValue("Type", (int)value | 0x100);
			}
		}
	}

	private void Installer_OnBeforeInstall(object source, InstallEventArgs args)
	{
		if (base.Context.Parameters.ContainsKey("u") || base.Context.Parameters.ContainsKey("U") || base.Context.Parameters.ContainsKey("uninstall") || base.Context.Parameters.ContainsKey("UNINSTALL") || !base.Context.Parameters.ContainsKey("account".ToLowerInvariant()))
		{
			return;
		}
		switch (base.Context.Parameters["account"].ToLowerInvariant())
		{
		case "localsystem":
			serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
			break;
		case "localservice":
			serviceProcessInstaller1.Account = ServiceAccount.LocalService;
			break;
		case "networkservice":
			serviceProcessInstaller1.Account = ServiceAccount.NetworkService;
			break;
		case "user":
			serviceProcessInstaller1.Account = ServiceAccount.User;
			if (base.Context.Parameters.ContainsKey("username") && base.Context.Parameters.ContainsKey("password"))
			{
				serviceProcessInstaller1.Username = base.Context.Parameters["username"];
				if (base.Context.Parameters.ContainsKey("IsPasswordEncoded"))
				{
					if (bool.TryParse(base.Context.Parameters["IsPasswordEncoded"], out var result) && result)
					{
						byte[] bytes = Convert.FromBase64String(base.Context.Parameters["password"]);
						serviceProcessInstaller1.Password = Encoding.Unicode.GetString(bytes);
						base.Context.Parameters["password"] = Encoding.Unicode.GetString(bytes);
					}
				}
				else
				{
					serviceProcessInstaller1.Password = base.Context.Parameters["password"];
				}
			}
			else
			{
				serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
			}
			break;
		}
	}

	private void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e)
	{
		try
		{
			EventLog.DeleteEventSource(OemResources.Instance.Server.ServiceName);
		}
		catch (SecurityException)
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		serviceProcessInstaller1 = new ServiceProcessInstaller();
		serviceInstaller1 = new ServiceInstallerEx();
		serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
		serviceProcessInstaller1.Password = null;
		serviceProcessInstaller1.Username = null;
		serviceInstaller1.Path = "";
		serviceInstaller1.ServiceName = "ServerTest";
		base.Installers.AddRange(new Installer[2] { serviceProcessInstaller1, serviceInstaller1 });
	}
}
