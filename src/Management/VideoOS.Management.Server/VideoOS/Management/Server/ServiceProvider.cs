using System;
using System.Collections.Generic;
using System.Threading;

namespace VideoOS.Management.Server;

public class ServiceProvider
{
	private Dictionary<Type, object> _serviceSyncObjects = new Dictionary<Type, object>();

	private ReaderWriterLock _serviceLock = new ReaderWriterLock();

	private Dictionary<Type, IGeminiServerService> _managedServices = new Dictionary<Type, IGeminiServerService>();

	public T GetService<T>(GeminiRequestContext requestContext) where T : class, IGeminiServerService
	{
		Type typeFromHandle = typeof(T);
		IGeminiServerService geminiServerService = LookupService(typeFromHandle);
		if (geminiServerService != null)
		{
			return (T)geminiServerService;
		}
		lock (AcquireServiceSyncObject(typeFromHandle))
		{
			geminiServerService = LookupService(typeFromHandle);
			if (geminiServerService != null)
			{
				return (T)geminiServerService;
			}
			geminiServerService = CreateAndStartService(requestContext, typeFromHandle);
			RegisterService(typeFromHandle, geminiServerService);
		}
		return (T)geminiServerService;
	}

	private object AcquireServiceSyncObject(Type serviceType)
	{
		object value;
		lock (_serviceSyncObjects)
		{
			if (!_serviceSyncObjects.TryGetValue(serviceType, out value))
			{
				value = new object();
				_serviceSyncObjects.Add(serviceType, value);
			}
		}
		return value;
	}

	private IGeminiServerService LookupService(Type serviceType)
	{
		try
		{
			_serviceLock.AcquireReaderLock(-1);
			if (_managedServices.TryGetValue(serviceType, out var value))
			{
				return value;
			}
		}
		finally
		{
			if (_serviceLock.IsReaderLockHeld)
			{
				_serviceLock.ReleaseReaderLock();
			}
		}
		return null;
	}

	private void RegisterService(Type serviceType, IGeminiServerService service)
	{
		try
		{
			_serviceLock.AcquireWriterLock(-1);
			_managedServices.Add(serviceType, service);
		}
		finally
		{
			if (_serviceLock.IsWriterLockHeld)
			{
				_serviceLock.ReleaseWriterLock();
			}
		}
	}

	private static IGeminiServerService CreateAndStartService(IGeminiRequestContext requestContext, Type serviceType)
	{
		IGeminiServerService geminiServerService;
		try
		{
			geminiServerService = (IGeminiServerService)Activator.CreateInstance(serviceType, nonPublic: true);
		}
		catch (MissingMethodException)
		{
			throw new ArgumentException("***PENDING*** Invalid type...", "serviceType");
		}
		try
		{
			geminiServerService.StartService(requestContext);
			return geminiServerService;
		}
		catch (Exception)
		{
			geminiServerService.StopService(requestContext);
			throw;
		}
	}
}
