using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using VideoOS.Platform.License;

namespace VideoOS.Platform.Proxy;

public class LicenseServiceProxy
{
	private static Guid _licenseServiceGuid = new Guid("6B584B34-BFCD-4619-A9DB-F5A38474125D");

	private static LicenseServiceClient _licenseServiceClient = null;

	private static void InitCheck()
	{
		try
		{
			if (_licenseServiceClient == null || _licenseServiceClient.State != CommunicationState.Opened)
			{
				EndpointAddress remoteAddress = new EndpointAddress(GetServiceUrl());
				_licenseServiceClient = new LicenseServiceClient(new BasicHttpBinding(), remoteAddress);
				_licenseServiceClient.InnerChannel.Open();
				long num = DateTime.Now.Ticks + 1000000;
				while (DateTime.Now.Ticks < num && _licenseServiceClient.State == CommunicationState.Opening)
				{
					Thread.Sleep(5);
				}
			}
		}
		catch (Exception innerException)
		{
			throw new LicenseServiceMIPException("Unable to connect to LicenseService", innerException);
		}
	}

	private static string GetServiceUrl()
	{
		List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(Configuration.MAPServiceId, Configuration.Instance.ServerFQID.ServerId);
		if (registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray.Count == 0)
		{
			throw new MIPException("Event server not found in registered services");
		}
		Uri uri = new UriBuilder(registeredServiceUriInfo[0].UriArray[0]).Uri;
		return uri.Scheme + "://" + uri.Host + string.Format(CultureInfo.InvariantCulture, ":{0}/LicenseService", uri.Port);
	}

	public static Guid RegisterConcurrentUse(ConcurrentLicenseUsed concurrentLicenseUsed)
	{
		try
		{
			InitCheck();
			return _licenseServiceClient.RegisterConcurrentUse(concurrentLicenseUsed);
		}
		catch (Exception)
		{
			_licenseServiceClient = null;
			InitCheck();
			try
			{
				return _licenseServiceClient.RegisterConcurrentUse(concurrentLicenseUsed);
			}
			catch (Exception ex2)
			{
				throw new LicenseServiceMIPException(ex2.Message, ex2);
			}
		}
	}

	public static void ReRegisterConcurrentUse(Guid registerId)
	{
		try
		{
			InitCheck();
			_licenseServiceClient.ReRegisterConcurrentUse(registerId);
		}
		catch (Exception)
		{
			_licenseServiceClient = null;
			try
			{
				InitCheck();
				_licenseServiceClient.ReRegisterConcurrentUse(registerId);
			}
			catch (Exception ex2)
			{
				throw new LicenseServiceMIPException(ex2.Message, ex2);
			}
		}
	}

	public static void UnRegisterConcurrentUse(Guid registerId)
	{
		try
		{
			InitCheck();
			_licenseServiceClient.UnRegisterConcurrentUse(registerId);
		}
		catch (Exception)
		{
			_licenseServiceClient = null;
			try
			{
				InitCheck();
				_licenseServiceClient.UnRegisterConcurrentUse(registerId);
			}
			catch (Exception ex2)
			{
				throw new LicenseServiceMIPException(ex2.Message, ex2);
			}
		}
	}

	public static ConcurrentLicenseUsed[] GetConcurrentLicenses(Guid pluginId, string licenseType)
	{
		try
		{
			InitCheck();
			return _licenseServiceClient.GetConcurrentLicenses(pluginId, licenseType);
		}
		catch (Exception)
		{
			_licenseServiceClient = null;
			try
			{
				InitCheck();
				return _licenseServiceClient.GetConcurrentLicenses(pluginId, licenseType);
			}
			catch (Exception ex2)
			{
				throw new LicenseServiceMIPException(ex2.Message, ex2);
			}
		}
	}
}
