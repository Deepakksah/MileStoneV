using System;
using System.ComponentModel;
using System.Globalization;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Web.Administration;
using VideoOS.Installer.Common;
using VideoOS.Installer.StartIIS7.Properties;

namespace VideoOS.Installer.StartIIS7;

public static class Program
{
	[STAThread]
	public static int Main(string[] args)
	{
		System.Windows.Forms.Application.EnableVisualStyles();
		System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		if (args.Length != 1)
		{
			return -1;
		}
		try
		{
			ServiceController serviceController = new ServiceController("W3SVC");
			if (serviceController.Status == ServiceControllerStatus.Stopped)
			{
				serviceController.Start();
				int num = 30;
				do
				{
					Thread.Sleep(1000);
					num--;
					serviceController.Refresh();
				}
				while (serviceController.Status != ServiceControllerStatus.Running && num > 0);
				if (serviceController.Status != ServiceControllerStatus.Running)
				{
					return -2;
				}
			}
		}
		catch (Win32Exception ex)
		{
			MessageHelper.ShowInstallerMessageBox(Resources.StartIIS7_ErrorOccurred, string.Format("{0}", ex.Message, CultureInfo.CurrentUICulture), InstallerMessageBoxIcon.Error);
			return -2;
		}
		catch (Exception ex2)
		{
			MessageHelper.ShowInstallerMessageBox(Resources.StartIIS7_ErrorOccurred, string.Format("{0}", ex2.Message, CultureInfo.CurrentUICulture), InstallerMessageBoxIcon.Error);
			return -2;
		}
		Site site = new ServerManager().Sites[args[0]];
		if (site.State == ObjectState.Stopped)
		{
			try
			{
				site.ServerAutoStart = true;
				site.Start();
				int num2 = 30;
				do
				{
					Thread.Sleep(1000);
					num2--;
				}
				while (site.State != ObjectState.Started && num2 > 0);
			}
			catch (ServerManagerException ex3)
			{
				MessageHelper.ShowInstallerMessageBox(Resources.StartIIS7_ErrorOccurred, string.Format("{0}", ex3.Message, CultureInfo.CurrentUICulture), InstallerMessageBoxIcon.Error);
				return -3;
			}
			catch (Exception ex4)
			{
				MessageHelper.ShowInstallerMessageBox(Resources.StartIIS7_ErrorOccurred, string.Format("{0}", ex4.Message, CultureInfo.CurrentUICulture), InstallerMessageBoxIcon.Error);
				return -4;
			}
		}
		return 0;
	}
}
