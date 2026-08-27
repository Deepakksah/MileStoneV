using System;
using System.Diagnostics;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public class RecordingServerCreateOptions
{
	private string defaultStorageName;

	private string defaultStoragePath;

	private int defaultStorageRetainMinutes;

	private long defaultStorageMaxSize;

	private int defaultFailoverUdpPort;

	public string DefaultStorageName
	{
		[DebuggerStepThrough]
		get
		{
			return defaultStorageName;
		}
		[DebuggerStepThrough]
		set
		{
			ValidateStringNotNullOrEmpty(value, "value");
			defaultStorageName = value;
		}
	}

	public string DefaultStoragePath
	{
		[DebuggerStepThrough]
		get
		{
			return defaultStoragePath;
		}
		[DebuggerStepThrough]
		set
		{
			ValidateStringNotNullOrEmpty(value, "value");
			defaultStoragePath = value;
		}
	}

	public int DefaultStorageRetainMinutes
	{
		[DebuggerStepThrough]
		get
		{
			return defaultStorageRetainMinutes;
		}
		[DebuggerStepThrough]
		set
		{
			ValidateRetainMinutes(value, "value");
			defaultStorageRetainMinutes = value;
		}
	}

	public long DefaultStorageMaxSize
	{
		[DebuggerStepThrough]
		get
		{
			return defaultStorageMaxSize;
		}
		[DebuggerStepThrough]
		set
		{
			ValidateMaxSize(value, "value");
			defaultStorageMaxSize = value;
		}
	}

	public int DefaultFailoverUdpPort
	{
		[DebuggerStepThrough]
		get
		{
			return defaultFailoverUdpPort;
		}
		[DebuggerStepThrough]
		set
		{
			ValidatePortNumber(value, "value");
			defaultFailoverUdpPort = value;
		}
	}

	public RecordingServerCreateOptions()
		: this("Local default", "C:\\MediaDatabase", (int)TimeSpan.FromDays(7.0).TotalMinutes, 1024000L)
	{
	}

	public RecordingServerCreateOptions(string defaultStorageName, string defaultStoragePath, int defaultStorageRetainMinutes, long defaultStorageMaxSize)
	{
		ValidateStringNotNullOrEmpty(defaultStorageName, "defaultStorageName");
		ValidateStringNotNullOrEmpty(defaultStoragePath, "defaultStoragePath");
		ValidateRetainMinutes(defaultStorageRetainMinutes, "defaultStorageRetainMinutes");
		ValidateMaxSize(defaultStorageMaxSize, "defaultStorageMaxSize");
		this.defaultStorageName = defaultStorageName;
		this.defaultStoragePath = defaultStoragePath;
		this.defaultStorageRetainMinutes = defaultStorageRetainMinutes;
		this.defaultStorageMaxSize = defaultStorageMaxSize;
	}

	private static void ValidateStringNotNullOrEmpty(string value, string parameterName)
	{
		if (value == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentException("The value cannot be an empty string.", parameterName);
		}
	}

	private static void ValidateRetainMinutes(int retainMinutes, string parameterName)
	{
		if (retainMinutes < 1 || retainMinutes > 525600000)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO62004);
		}
	}

	private static void ValidateMaxSize(long maxSize, string parameterName)
	{
		if (maxSize < 1 || maxSize > 1099511627776L)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO62005);
		}
	}

	private static void ValidatePortNumber(long value, string parameterName)
	{
		if (value < 1 || value > 65535)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60183);
		}
	}
}
