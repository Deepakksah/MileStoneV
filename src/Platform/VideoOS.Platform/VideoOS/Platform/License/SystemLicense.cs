using System;
using System.Collections.Generic;

namespace VideoOS.Platform.License;

public class SystemLicense
{
	private string _SLC = "";

	private DateTime _expire = DateTime.MinValue;

	public virtual string SLC
	{
		get
		{
			try
			{
				_SLC = EnvironmentManager.Instance.LicenseManager.SLC;
				return _SLC;
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: false, "SLC", ex.Message, null);
				return "";
			}
		}
		public set
		{
			_SLC = value;
		}
	}

	public virtual DateTime Issued { get; public set; }

	public virtual int ProductCode
	{
		get
		{
			return EnvironmentManager.Instance.LicenseManager.ProductCode;
		}
		set
		{
		}
	}

	public virtual DateTime Expire
	{
		get
		{
			try
			{
				return EnvironmentManager.Instance.LicenseManager.Expire;
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: false, "SystemLicense.Expire", ex.Message, null);
				return _expire;
			}
		}
		public set
		{
			_expire = value;
		}
	}

	public virtual int GracePeriod { get; public set; }

	public virtual string CudaPublicKey { get; set; }

	public string ProductionEnvironment => EnvironmentManager.Instance.LicenseManager.ProductionEnvironment;

	public virtual List<string> FeatureFlags
	{
		get
		{
			try
			{
				EnvironmentManager.Instance.LicenseManager.GetLicense(Guid.Empty);
				return EnvironmentManager.Instance.LicenseManager.Features;
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: false, "IsFeatureEnabled", ex.Message, null);
				return new List<string>();
			}
		}
	}

	public virtual Dictionary<string, DateTime> ExpirationDateTimes
	{
		get
		{
			try
			{
				EnvironmentManager.Instance.LicenseManager.GetLicense(Guid.Empty);
				return EnvironmentManager.Instance.LicenseManager.GetExpirationDateTimes;
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: false, "ExpirationDateTimes", ex.Message, null);
				return new Dictionary<string, DateTime>();
			}
		}
	}

	public virtual bool IsFeatureEnabled(string featureName)
	{
		try
		{
			if (EnvironmentManager.Instance.MasterSite.ServerId.ServerType == "XPCO" && EnvironmentManager.Instance.LicenseManager.Features.Count == 0)
			{
				return true;
			}
			if (EnvironmentManager.Instance.SmartClientInOfflineMode && EnvironmentManager.Instance.LicenseManager.Features.Count == 0)
			{
				return true;
			}
			if (EnvironmentManager.Instance.LicenseManager.Features.Contains(featureName))
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "IsFeatureEnabled", ex.Message, null);
			return false;
		}
	}
}
