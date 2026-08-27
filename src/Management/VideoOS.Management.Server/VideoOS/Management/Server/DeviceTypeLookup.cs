using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Caching;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class DeviceTypeLookup : IDeviceTypeLookup, IDisposable
{
	private static object instanceLock = new object();

	private static volatile DeviceTypeLookup instance;

	private MemoryCache deviceTypeCache = new MemoryCache("DeviceTypeCache");

	private Dictionary<string, DeviceType> deviceSecurityAttributeDeviceTypeMap = new Dictionary<string, DeviceType>();

	public static DeviceTypeLookup Instance
	{
		[DebuggerStepThrough]
		get
		{
			if (instance != null)
			{
				return instance;
			}
			lock (instanceLock)
			{
				if (instance == null)
				{
					instance = new DeviceTypeLookup();
				}
			}
			return instance;
		}
	}

	public DeviceTypeLookup()
	{
		ResolveSecurityAttributes(typeof(BuiltInPermissions.DeviceSecurity.Camera), DeviceType.Camera);
		ResolveSecurityAttributes(typeof(BuiltInPermissions.Ptz), DeviceType.Camera);
		ResolveSecurityAttributes(typeof(BuiltInPermissions.DeviceSecurity.Microphone), DeviceType.Microphone);
		ResolveSecurityAttributes(typeof(BuiltInPermissions.DeviceSecurity.Speaker), DeviceType.Speaker);
		ResolveSecurityAttributes(typeof(BuiltInPermissions.Speak), DeviceType.Speaker);
		ResolveSecurityAttributes(typeof(BuiltInPermissions.DeviceSecurity.Input), DeviceType.Input);
		ResolveSecurityAttributes(typeof(BuiltInPermissions.DeviceSecurity.Output), DeviceType.Output);
		ResolveSecurityAttributes(typeof(BuiltInPermissions.DeviceSecurity.Metadata), DeviceType.Metadata);
	}

	private void ResolveSecurityAttributes(Type type, DeviceType deviceType)
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < fields.Length; i++)
		{
			if (fields[i].GetValue(null) is string key)
			{
				deviceSecurityAttributeDeviceTypeMap.Add(key, deviceType);
			}
		}
	}

	public DeviceType QueryDeviceTypeFromSecurityAttributeValue(string securityAttributeValue)
	{
		if (!deviceSecurityAttributeDeviceTypeMap.TryGetValue(securityAttributeValue, out var value))
		{
			return DeviceType.Unknown;
		}
		return value;
	}

	public DeviceType QueryDeviceTypeFromDeviceId(IGeminiRequestContext requestContext, string deviceId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		object obj = deviceTypeCache.Get(deviceId.ToUpperInvariant());
		if (obj != null)
		{
			return (DeviceType)obj;
		}
		try
		{
			Device device = null;
			using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
			{
				device = deviceComponent.QueryDeviceById(deviceId);
			}
			if (device == null)
			{
				return DeviceType.Unknown;
			}
			CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
			cacheItemPolicy.SlidingExpiration = ObjectCache.NoSlidingExpiration;
			deviceTypeCache.Add(deviceId.ToUpperInvariant(), device.DeviceType, cacheItemPolicy);
			return device.DeviceType;
		}
		catch (DatabaseException innerException)
		{
			throw new DatabaseConnectionException(innerException);
		}
	}

	public Guid QueryDeviceSecurityNamespaceId(IGeminiRequestContext requestContext, string deviceId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (deviceId == null)
		{
			throw new ArgumentNullException("deviceId");
		}
		DeviceType deviceType;
		try
		{
			deviceType = QueryDeviceTypeFromDeviceId(requestContext, deviceId);
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		if (deviceType == DeviceType.Unknown)
		{
			throw new UnknownDeviceException("Device could not be found. Id: " + deviceId);
		}
		return DeviceTypeInformation.DeviceTypeDictionary[deviceType].SecurityNamespace;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && deviceTypeCache != null)
		{
			deviceTypeCache.Dispose();
			deviceTypeCache = null;
		}
	}
}
