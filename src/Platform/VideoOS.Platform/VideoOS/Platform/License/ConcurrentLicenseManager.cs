using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy;

namespace VideoOS.Platform.License;

public class ConcurrentLicenseManager
{
	private Thread _refreshThread;

	private Collection<Guid> _registeredGuids = new Collection<Guid>();

	public void SaveLicenseInformation(LicenseInformation licenseInformation)
	{
		if (EnvironmentManager.Instance.Mode != Mode.ServerSetup)
		{
			throw new ImplementationMIPException("Only supported in ServerSetup mode");
		}
		EnvironmentManager.Instance.LicenseManager.SaveLicenseInformation(licenseInformation);
	}

	public LicenseInformation GetLicenseInformation(Guid pluginId, string licenseType)
	{
		return EnvironmentManager.Instance.LicenseManager.GetLicenseInformation(pluginId, licenseType);
	}

	public ConcurrentLicenseUsed[] GetConcurrentUseList(Guid pluginId, string licenseType)
	{
		return LicenseServiceProxy.GetConcurrentLicenses(pluginId, licenseType);
	}

	public Guid RegisterConcurrentLicense(Guid pluginId, string licenseType)
	{
		ConcurrentLicenseUsed concurrentLicenseUsed = new ConcurrentLicenseUsed
		{
			PluginId = pluginId,
			LicenseType = licenseType,
			MachineName = Environment.MachineName
		};
		LoginSettings loginSettings = LoginSettings.GetLoginSettings(EnvironmentManager.Instance.MasterSite.ServerId);
		if (loginSettings.UserName != "")
		{
			concurrentLicenseUsed.WindowsUser = loginSettings.UserName;
		}
		else
		{
			concurrentLicenseUsed.WindowsUser = Environment.UserName;
		}
		concurrentLicenseUsed.IPAddress = "";
		IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
		foreach (IPAddress iPAddress in addressList)
		{
			if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
			{
				concurrentLicenseUsed.IPAddress = iPAddress.ToString();
				break;
			}
		}
		Guid guid = LicenseServiceProxy.RegisterConcurrentUse(concurrentLicenseUsed);
		lock (_registeredGuids)
		{
			_registeredGuids.Add(guid);
		}
		if (_refreshThread == null)
		{
			_refreshThread = new Thread(RunRefresh);
			_refreshThread.IsBackground = true;
			_refreshThread.Start();
		}
		return guid;
	}

	public void UnRegisterConcurrentLicense(Guid registerGuid)
	{
		LicenseServiceProxy.UnRegisterConcurrentUse(registerGuid);
		lock (_registeredGuids)
		{
			if (_registeredGuids.Contains(registerGuid))
			{
				_registeredGuids.Remove(registerGuid);
			}
		}
	}

	private void RunRefresh()
	{
		try
		{
			DateTime dateTime = DateTime.Now + TimeSpan.FromMinutes(4.0);
			while (EnvironmentManager.Instance.ApplicationLoggedOn && _registeredGuids.Count > 0)
			{
				Thread.Sleep(2000);
				if (!(DateTime.Now > dateTime))
				{
					continue;
				}
				lock (_registeredGuids)
				{
					foreach (Guid registeredGuid in _registeredGuids)
					{
						LicenseServiceProxy.ReRegisterConcurrentUse(registeredGuid);
					}
				}
				dateTime = DateTime.Now + TimeSpan.FromMinutes(4.0);
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string text = "LicenseRefresh stopped" + ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "ConcurrentLicenseManager", text, args);
		}
	}
}
