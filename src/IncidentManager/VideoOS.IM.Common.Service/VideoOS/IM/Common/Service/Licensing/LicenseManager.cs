using System;
using System.Threading;
using VideoOS.IM.Common.GlobalDefinitions.Utilities;
using VideoOS.IM.Common.Service.Interfaces.Licensing;
using VideoOS.Platform;
using VideoOS.Platform.License;

namespace VideoOS.IM.Common.Service.Licensing;

public class LicenseManager : ILicenseManager
{
	private static readonly Lazy<LicenseManager> _instance = new Lazy<LicenseManager>(() => new LicenseManager());

	private ReaderWriterLockSlim _licenseLock = new ReaderWriterLockSlim();

	public static LicenseManager Instance => _instance.Value;

	public LicenseInformation LicenseRequest
	{
		get
		{
			_licenseLock.EnterReadLock();
			try
			{
				return new LicenseInformation
				{
					PluginId = Constants.IncidentManagerPluginId,
					LicenseType = Constants.IncidentManagerLicenseId,
					CounterMethod = LicenseInformation.CounterMethodEnum.Auto,
					CustomData = ""
				};
			}
			finally
			{
				_licenseLock.ExitReadLock();
			}
		}
	}

	public LicenseInformation LicenseInfo => EnvironmentManager.Instance.LicenseManager.GetLicense(Constants.IncidentManagerPluginId, Constants.IncidentManagerLicenseId);

	public bool IsLicenseExpired => EnvironmentManager.Instance.LicenseManager.GetLicense(Constants.IncidentManagerPluginId, Constants.IncidentManagerLicenseId).Expire.ToUniversalTime() < DateTime.Now.ToUniversalTime();

	private LicenseManager()
	{
	}
}
