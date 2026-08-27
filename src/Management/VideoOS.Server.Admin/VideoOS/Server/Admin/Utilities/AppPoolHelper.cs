using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;

namespace VideoOS.Server.Admin.Utilities;

public class AppPoolHelper : IDisposable
{
	public enum StartMode
	{
		OnDemand,
		AlwaysRunning
	}

	private readonly ServerManager _serverManager;

	private bool disposedValue;

	public ILogger LogComponent { get; public set; }

	public AppPoolHelper()
	{
		_serverManager = new ServerManager();
	}

	public AppPoolHelper(ILogger logComponent)
		: this()
	{
		LogComponent = logComponent;
	}

	public void DeleteApplicationPool(ApplicationPool appPoolToDelete)
	{
		if (appPoolToDelete == null)
		{
			throw new ArgumentException(AppPoolHelper_Resources.Application_pool_to_delete_is_null, "appPoolToDelete");
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, AppPoolHelper_Resources.Deleting_application_pool_Name, appPoolToDelete.Name), Array.Empty<object>());
		_serverManager.ApplicationPools.Remove(appPoolToDelete);
		_serverManager.CommitChanges();
	}

	public void CreateApplicationPool(string newApplicationPoolName, AccountInfo account, bool classicManagedPipelineMode, StartMode startMode, string managedRuntimeVersion, bool rapidFailProtection)
	{
		if (account == null)
		{
			throw new ArgumentException("Account is null", "account");
		}
		if (managedRuntimeVersion == null)
		{
			throw new ArgumentException("Managed Runtime Version is null", "managedRuntimeVersion");
		}
		if (string.IsNullOrEmpty(newApplicationPoolName))
		{
			throw new ArgumentException(AppPoolHelper_Resources.AppPool_name_is_null, "newApplicationPoolName");
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, AppPoolHelper_Resources.Creating_new_application_pool_Named, newApplicationPoolName), Array.Empty<object>());
		ApplicationPool applicationPool = FindOrCreateAppPool(newApplicationPoolName);
		SetAppPoolSecurity(applicationPool, account);
		SetAppPooolStartMode(applicationPool, startMode);
		SetManagedRuntimeVersionForApplicationPool(applicationPool, classicManagedPipelineMode, managedRuntimeVersion);
		if (!rapidFailProtection)
		{
			applicationPool.Failure.RapidFailProtection = false;
		}
		_serverManager.CommitChanges();
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, AppPoolHelper_Resources.Application_pool_created_Name, newApplicationPoolName), Array.Empty<object>());
	}

	public void CreateApplicationPool(string newApplicationPoolName, AccountInfo account, bool classicManagedPipelineMode, StartMode startMode, string managedRuntimeVersion)
	{
		CreateApplicationPool(newApplicationPoolName, account, classicManagedPipelineMode, startMode, managedRuntimeVersion, rapidFailProtection: true);
	}

	public static ObjectState RecycleApplicationPool(string appPoolName)
	{
		if (appPoolName == null)
		{
			throw new ArgumentNullException("appPoolName");
		}
		using ServerManager serverManager = new ServerManager();
		ObjectState result = (serverManager.ApplicationPools.FirstOrDefault((ApplicationPool a) => appPoolName.Equals(a.Name, StringComparison.Ordinal)) ?? throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, AppPoolHelper_Resources.Application_pool_not_found, appPoolName))).Recycle();
		serverManager.CommitChanges();
		return result;
	}

	public static ObjectState GetAppPoolState(string appPoolName)
	{
		using ServerManager serverManager = new ServerManager();
		ApplicationPool applicationPool = serverManager.ApplicationPools.FirstOrDefault((ApplicationPool x) => x.Name.Equals(appPoolName, StringComparison.OrdinalIgnoreCase));
		if (applicationPool == null)
		{
			throw new ArgumentException(FormattableString.Invariant($"Apppool with the name {appPoolName} was not found"));
		}
		return applicationPool.State;
	}

	public static void SetAppPoolSecurity(ApplicationPool pool, AccountInfo account)
	{
		pool.ProcessModel.IdentityType = ConvertIdentity(account.ServiceAccountType);
		if (pool.ProcessModel.IdentityType == ProcessModelIdentityType.SpecificUser)
		{
			if (!string.IsNullOrEmpty(account.Domain))
			{
				pool.ProcessModel.UserName = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", account.Domain, account.UserName);
			}
			else
			{
				pool.ProcessModel.UserName = account.UserName;
			}
		}
		if (account.Password != null)
		{
			pool.ProcessModel.Password = account.Password;
		}
	}

	private static ProcessModelIdentityType ConvertIdentity(ServiceAccount serviceAccountType)
	{
		return serviceAccountType switch
		{
			ServiceAccount.LocalService => ProcessModelIdentityType.LocalService, 
			ServiceAccount.NetworkService => ProcessModelIdentityType.NetworkService, 
			ServiceAccount.LocalSystem => ProcessModelIdentityType.LocalSystem, 
			ServiceAccount.User => ProcessModelIdentityType.SpecificUser, 
			_ => throw new ArgumentOutOfRangeException(FormattableString.Invariant(FormattableStringFactory.Create("Unexcpected value of {0} : {1}", "serviceAccountType", serviceAccountType))), 
		};
	}

	public static ApplicationPool GetAppPool(string appPoolName, ServerManager serverManager)
	{
		return serverManager.ApplicationPools.FirstOrDefault((ApplicationPool x) => x.Name.Equals(appPoolName, StringComparison.OrdinalIgnoreCase));
	}

	public ApplicationPool GetAppPool(string appPoolName)
	{
		return GetAppPool(appPoolName, _serverManager);
	}

	public ApplicationPool FindOrCreateAppPool(string appPoolName)
	{
		ApplicationPool appPool = GetAppPool(appPoolName, _serverManager);
		if (appPool != null)
		{
			DeleteApplicationPool(appPool);
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, AppPoolHelper_Resources.Deleting_existing_application_pool), Array.Empty<object>());
		}
		return _serverManager.ApplicationPools.Add(appPoolName);
	}

	private void SetManagedRuntimeVersionForApplicationPool(ApplicationPool appPool, bool classicManagedPipelineMode, string managedRuntimeVersion)
	{
		if (appPool == null)
		{
			throw new ArgumentException(AppPoolHelper_Resources.AppPool_name_is_null, "appPool");
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, AppPoolHelper_Resources.Setting_the_managed_runtime_version_for_the_application_pool), Array.Empty<object>());
		if (appPool != null)
		{
			appPool.ManagedRuntimeVersion = managedRuntimeVersion;
			appPool.ProcessModel.LoadUserProfile = true;
			if (classicManagedPipelineMode)
			{
				appPool.ManagedPipelineMode = ManagedPipelineMode.Classic;
			}
			else
			{
				appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
			}
		}
	}

	private void SetAppPooolStartMode(ApplicationPool appPool, StartMode startMode)
	{
		PropertyInfo property = typeof(ApplicationPool).GetProperty("StartMode");
		if (!(property == null))
		{
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, AppPoolHelper_Resources.Setting_start_mode_on_the_application_pool, appPool.Name), Array.Empty<object>());
			property.SetValue(appPool, startMode);
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				_serverManager.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
