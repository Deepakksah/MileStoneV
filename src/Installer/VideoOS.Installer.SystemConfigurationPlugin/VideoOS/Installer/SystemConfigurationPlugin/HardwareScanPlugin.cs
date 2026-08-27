using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.ConfigurationAPI;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class HardwareScanPlugin : ProgressPlugin
{
	public HardwareScanPlugin(int serverPort)
		: base(serverPort, 20)
	{
	}

	public ScanResults DoHardwareScan(bool useDefaultCredentials, string userName1, string password1, string userName2, string password2, string userName3, string password3, bool useHttpsScan, ConfigurationApiServiceManager serviceManager, CancellationToken cancellation)
	{
		OnProgressChanged(0, Resources.ScanningForHardware);
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		try
		{
			ConfigurationItem recorderItem = GetRecorderItem(serviceManager);
			ConfigurationItem task = StartScan(recorderItem, useDefaultCredentials, userName1, password1, userName2, password2, userName3, password3, useHttpsScan, serviceManager);
			task = EnsureScanningDone(task, serviceManager, cancellation);
			HardwareConfigurations scanResult = GetScanResult(task, recorderItem, serviceManager);
			return new ScanResults(ScanErrorCode.NoError, HardwareConfigurations.GetAsXml(scanResult));
		}
		catch (HardwareScanException ex)
		{
			if (!string.IsNullOrEmpty(ex.Message))
			{
				LogManager.Write("Scanning hardware exception: " + ex.Message);
			}
			return new ScanResults(ex.ErrorCode, HardwareConfigurations.GetAsXml(new HardwareConfigurations()));
		}
	}

	private static ConfigurationItem GetRecorderItem(ConfigurationApiServiceManager serviceManager)
	{
		ConfigurationItem[] array = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems("/RecordingServerFolder"), null);
		if (array.Length != 1)
		{
			throw new HardwareScanException(ScanErrorCode.Generic, "Invalid number of recorders when starting scan");
		}
		return array[0];
	}

	private static ConfigurationItem StartScan(ConfigurationItem recorderItem, bool useDefaultCredentials, string userName1, string password1, string userName2, string password2, string userName3, string password3, bool useHttpsScan, ConfigurationApiServiceManager serviceManager)
	{
		ConfigurationItem hardwareScanInvokeInfo = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(recorderItem, "HardwareScanExpress"), null);
		hardwareScanInvokeInfo.Properties.SetValue("UseDefaultCredentials", useDefaultCredentials);
		if (!string.IsNullOrEmpty(userName1))
		{
			hardwareScanInvokeInfo.Properties.SetValue("PrimaryUserName", userName1);
			hardwareScanInvokeInfo.Properties.SetValue("PrimaryPassword", password1);
			if (!string.IsNullOrEmpty(userName2))
			{
				hardwareScanInvokeInfo.Properties.SetValue("SecondaryUserName", userName2);
				hardwareScanInvokeInfo.Properties.SetValue("SecondaryPassword", password2);
				if (!string.IsNullOrEmpty(userName3))
				{
					hardwareScanInvokeInfo.Properties.SetValue("TertiaryUserName", userName3);
					hardwareScanInvokeInfo.Properties.SetValue("TertiaryPassword", password3);
				}
			}
		}
		hardwareScanInvokeInfo.Properties.SetValue("UseSecurePort", useHttpsScan);
		if (!serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(hardwareScanInvokeInfo, "HardwareScanExpress"), null).Properties.TryGetPropertyValue("Path", out var taskPath))
		{
			throw new HardwareScanException(ScanErrorCode.Generic, "Task path is not found");
		}
		return serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(taskPath), null);
	}

	private ConfigurationItem EnsureScanningDone(ConfigurationItem task, ConfigurationApiServiceManager serviceManager, CancellationToken token)
	{
		if (!task.Properties.TryGetPropertyValue("State", out var value))
		{
			throw new HardwareScanException(ScanErrorCode.Generic, "Cannot lookup scan state");
		}
		int num = int.MinValue;
		while (value.Equals("Idle", StringComparison.Ordinal) || value.Equals("InProgress", StringComparison.Ordinal))
		{
			int progress = GetProgress(task);
			if (progress >= 0 && progress != num)
			{
				num = progress;
				OnProgressChanged(progress, Resources.ScanningForHardware);
			}
			Thread.Sleep(500);
			task = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(task.Path), null);
			if (token.IsCancellationRequested)
			{
				LogManager.Write("Stopping task and retrieving discovered devices so far.");
				serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(task, "TaskStop"), null);
			}
			if (!task.Properties.TryGetPropertyValue("State", out value))
			{
				throw new HardwareScanException(ScanErrorCode.Generic, "Cannot lookup scan state after multiple iterations");
			}
		}
		return task;
	}

	private static HardwareConfigurations GetScanResult(ConfigurationItem task, ConfigurationItem recorderItem, ConfigurationApiServiceManager serviceManager)
	{
		if (!task.ChildrenFilled)
		{
			HandleEmptyScanResult(task);
		}
		return FillChildren(task, recorderItem, serviceManager);
	}

	private static void HandleEmptyScanResult(ConfigurationItem task)
	{
		if (!task.Properties.TryGetPropertyValue("State", out var value))
		{
			throw new HardwareScanException(ScanErrorCode.Generic, "Cannot lookup scan state on getting scan result");
		}
		if (value.Equals("Error", StringComparison.Ordinal) || value.Equals("Unknown", StringComparison.Ordinal))
		{
			string value2 = string.Empty;
			string value3 = string.Empty;
			task.Properties.TryGetPropertyValue("ErrorCode", out value2);
			task.Properties.TryGetPropertyValue("ErrorText", out value3);
			string message = "Scanning Hardware failed, Error message: " + value3 + " error code: " + value2;
			if (value2 == "60638")
			{
				throw new HardwareScanException(ScanErrorCode.NoHardwareFound, message);
			}
			if (value2 == "60631")
			{
				throw new HardwareScanException(ScanErrorCode.AlreadyRunning, message);
			}
			throw new HardwareScanException(ScanErrorCode.Generic, message);
		}
		throw new HardwareScanException(ScanErrorCode.Generic, "");
	}

	private static HardwareConfigurations FillChildren(ConfigurationItem task, ConfigurationItem recorderItem, ConfigurationApiServiceManager serviceManager)
	{
		HardwareConfigurations hardwareConfigurations = new HardwareConfigurations();
		ConfigurationItem[] array = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(recorderItem.Path), null);
		string hardwareDriverFolder = array[1].Path;
		ConfigurationItem[] hardwareDrivers = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(hardwareDriverFolder), null);
		ConfigurationItem[] children = task.Children;
		for (int num = 0; num < children.Length; num++)
		{
			HardwareConfiguration hardwareConfiguration = FillChild(children[num], hardwareDrivers);
			if (hardwareConfiguration != null)
			{
				hardwareConfigurations.Add(hardwareConfiguration);
			}
		}
		return hardwareConfigurations;
	}

	private static HardwareConfiguration FillChild(ConfigurationItem child, ConfigurationItem[] hardwareDrivers)
	{
		try
		{
			if (child.Properties.TryGetPropertyValue("HardwareDriverPath", out var driverPath))
			{
				ConfigurationItem configurationItem = hardwareDrivers.First((ConfigurationItem o) => o.Path == driverPath);
				return new HardwareConfiguration(child.Properties.First((Property f) => f.Key.Equals("UserName", StringComparison.OrdinalIgnoreCase)).Value, child.Properties.First((Property f) => f.Key.Equals("Password", StringComparison.OrdinalIgnoreCase)).Value, ipAddress: child.Properties.First((Property f) => f.Key.Equals("HardwareAddress", StringComparison.OrdinalIgnoreCase)).Value, validated: bool.Parse(child.Properties.First((Property f) => f.Key.Equals("HardwareScanValidated", StringComparison.OrdinalIgnoreCase)).Value), driverPath: driverPath, name: configurationItem.DisplayName);
			}
		}
		catch
		{
		}
		return null;
	}

	private static int GetProgress(ConfigurationItem task)
	{
		if (task != null)
		{
			Property property = task.Properties.FirstOrDefault((Property f) => f.Key.Equals("Progress", StringComparison.OrdinalIgnoreCase));
			if (property != null)
			{
				return int.Parse(property.Value, CultureInfo.InvariantCulture);
			}
		}
		return -1;
	}
}
