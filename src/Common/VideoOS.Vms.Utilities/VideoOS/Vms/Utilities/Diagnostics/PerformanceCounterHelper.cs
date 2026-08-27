using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace VideoOS.Vms.Utilities.Diagnostics;

public class PerformanceCounterHelper : IPerformanceCounterHelper, IDisposable
{
	public const int InstanceIdsMaxSize = 3;

	private const int ElipsisSize = 3;

	private const int InstanceNameMaxSize = 127;

	private const int DelimitorSize = 1;

	private object _lockObj = new object();

	private PerformanceCounter _counterInternal;

	private Guid[] _instanceIds;

	public string InstanceName { get; private set; }

	public long RawValue
	{
		get
		{
			if (_counterInternal != null)
			{
				lock (_lockObj)
				{
					if (_counterInternal != null)
					{
						return _counterInternal.RawValue;
					}
					return 0L;
				}
			}
			return 0L;
		}
		set
		{
			if (_counterInternal == null)
			{
				return;
			}
			lock (_lockObj)
			{
				if (_counterInternal != null)
				{
					_counterInternal.RawValue = value;
				}
			}
		}
	}

	public static event EventHandler<PerformanceCounterEventArgs> PerformanceCounterInstanceBeforeCreate;

	public Guid[] GetInstanceIds()
	{
		return _instanceIds.ToArray();
	}

	public static string CreateInstanceName(string instanceName)
	{
		return ReplaceIllegalCharacters(TruncateInstanceNameAsNeeded(instanceName));
	}

	private static string CreateInstanceName(string instanceName, params Guid[] instanceIds)
	{
		string text = string.Empty;
		for (int i = 0; i < instanceIds.Length; i++)
		{
			Guid guid = instanceIds[i];
			text += string.Format(CultureInfo.InvariantCulture, "[{0}]", guid.ToString());
		}
		if (text.Length >= 127)
		{
			throw new ArgumentException("The number of instance ids results in exceeding the maximum instance name size of 127 characters.", "instanceIds");
		}
		string text2 = string.Format(CultureInfo.InvariantCulture, "{0} {1}", instanceName, text);
		string instanceName2 = instanceName;
		if (text2.Length > 127)
		{
			instanceName2 = instanceName.Substring(0, 127 - text.Length - 3 - 1);
			instanceName2 += "...";
		}
		instanceName2 = ReplaceIllegalCharacters(instanceName2);
		return string.Format(CultureInfo.InvariantCulture, "{0} {1}", instanceName2, text);
	}

	public static PerformanceCounterHelper CreatePerformanceCounterHelper(string category, string counterQueueSize, string instanceName)
	{
		return CreatePerformanceCounterHelperInternal(category, counterQueueSize, instanceName, null, PerformanceCounterInstanceLifetime.Global);
	}

	public static PerformanceCounterHelper CreatePerformanceCounterHelper(string category, string counterQueueSize, string instanceName, long? rawValue)
	{
		return CreatePerformanceCounterHelperInternal(category, counterQueueSize, instanceName, rawValue, PerformanceCounterInstanceLifetime.Global);
	}

	private static PerformanceCounterHelper CreatePerformanceCounterHelperInternal(string category, string counterQueueSize, string instanceName, long? rawValue, PerformanceCounterInstanceLifetime instanceLifetime, params Guid[] instanceIds)
	{
		PerformanceCounterHelper performanceCounterHelper = null;
		PerformanceCounterHelper result = null;
		try
		{
			performanceCounterHelper = new PerformanceCounterHelper();
			performanceCounterHelper.CreateInstance(category, counterQueueSize, instanceName, instanceLifetime, instanceIds);
			if (rawValue.HasValue)
			{
				performanceCounterHelper.RawValue = rawValue.Value;
			}
			result = performanceCounterHelper;
			performanceCounterHelper = null;
		}
		catch (InvalidOperationException)
		{
		}
		catch (ArgumentNullException)
		{
		}
		catch (Win32Exception)
		{
		}
		catch (PlatformNotSupportedException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		finally
		{
			performanceCounterHelper?.Close();
		}
		return result;
	}

	public static PerformanceCounterHelper CreatePerformanceCounterHelper(string category, string counterQueueSize, string instanceName, params Guid[] instanceIds)
	{
		return CreatePerformanceCounterHelperInternal(category, counterQueueSize, instanceName, null, PerformanceCounterInstanceLifetime.Global, instanceIds);
	}

	public static PerformanceCounterHelper CreatePerformanceCounterHelper(string category, string counterQueueSize, string instanceName, long rawValue, params Guid[] instanceIds)
	{
		return CreatePerformanceCounterHelperInternal(category, counterQueueSize, instanceName, rawValue, PerformanceCounterInstanceLifetime.Global, instanceIds);
	}

	public static bool ValidateInstanceName(string instanceName)
	{
		if (instanceName == null)
		{
			throw new ArgumentNullException("instanceName");
		}
		if (instanceName.Length.Equals(0))
		{
			throw new ArgumentException("A performance counter instance name can't be empty.", "instanceName");
		}
		return instanceName.Length <= 127;
	}

	private static string TruncateInstanceNameAsNeeded(string instanceName)
	{
		string result = instanceName;
		if (!ValidateInstanceName(instanceName) && instanceName.Length + 1 > 127)
		{
			Match match = Regex.Match(instanceName, "(?<instanceName>.+)(?<instanceId>\\[[^\\[\\]]+\\]$)");
			if (!match.Success)
			{
				throw new ArgumentException("A performance counter instance name is invalid.", "instanceName");
			}
			string arg = match.Groups["instanceName"].Value.Trim().Substring(0, 127 - match.Groups["instanceId"].Value.Length - 3 - 1);
			result = string.Format(CultureInfo.InvariantCulture, "{0}... {1}", arg, match.Groups["instanceId"].Value);
		}
		return result;
	}

	private static string ReplaceIllegalCharacters(string instanceName)
	{
		return instanceName.Replace('(', '{').Replace(')', '}').Replace('#', '-');
	}

	public bool CreateInstance(string categoryName, string counterName, string instanceName, PerformanceCounterInstanceLifetime instanceLifetime, params Guid[] instanceIds)
	{
		string instanceName2;
		if (instanceIds != null && instanceIds.Length != 0)
		{
			instanceName2 = CreateInstanceName(instanceName, instanceIds);
			_instanceIds = instanceIds;
		}
		else
		{
			instanceName2 = CreateInstanceName(instanceName);
		}
		InstanceName = instanceName;
		try
		{
			if (_counterInternal != null)
			{
				_counterInternal.RemoveInstance();
				_counterInternal.Dispose();
			}
			else if (instanceIds != null)
			{
				OnBeforePerformanceCounterInstanceCreate(categoryName, counterName, instanceName2, Array.ConvertAll(instanceIds, (Guid x) => x.ToString()));
			}
			if (PerformanceCounterCategory.Exists(categoryName))
			{
				_counterInternal = new PerformanceCounter();
				_counterInternal.CategoryName = categoryName;
				_counterInternal.CounterName = counterName;
				_counterInternal.InstanceName = instanceName2;
				_counterInternal.ReadOnly = false;
				_counterInternal.InstanceLifetime = instanceLifetime;
			}
		}
		catch (InvalidOperationException)
		{
		}
		catch (Win32Exception)
		{
		}
		catch (PlatformNotSupportedException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		return _counterInternal != null;
	}

	protected virtual void OnBeforePerformanceCounterInstanceCreate(string categoryName, string counterName, string instanceName, IEnumerable<string> instanceIds)
	{
		PerformanceCounterHelper.PerformanceCounterInstanceBeforeCreate?.Invoke(this, new PerformanceCounterEventArgs(categoryName, counterName, instanceName, instanceIds));
	}

	public long Increment()
	{
		try
		{
			if (_counterInternal != null)
			{
				lock (_lockObj)
				{
					if (_counterInternal != null)
					{
						return _counterInternal.Increment();
					}
				}
			}
		}
		catch (InvalidOperationException)
		{
		}
		catch (Win32Exception)
		{
		}
		catch (PlatformNotSupportedException)
		{
		}
		return 0L;
	}

	public long IncrementBy(long value)
	{
		try
		{
			if (_counterInternal != null)
			{
				lock (_lockObj)
				{
					if (_counterInternal != null)
					{
						return _counterInternal.IncrementBy(value);
					}
				}
			}
		}
		catch (InvalidOperationException)
		{
		}
		catch (Win32Exception)
		{
		}
		catch (PlatformNotSupportedException)
		{
		}
		return 0L;
	}

	public long Decrement()
	{
		try
		{
			if (_counterInternal != null)
			{
				lock (_lockObj)
				{
					if (_counterInternal != null)
					{
						return _counterInternal.Decrement();
					}
				}
			}
		}
		catch (InvalidOperationException)
		{
		}
		catch (Win32Exception)
		{
		}
		catch (PlatformNotSupportedException)
		{
		}
		return 0L;
	}

	public void UpdateInstanceName(string newInstanceName)
	{
		if (_counterInternal == null || InstanceName.Equals(newInstanceName))
		{
			return;
		}
		lock (_lockObj)
		{
			long rawValue = _counterInternal.RawValue;
			_counterInternal.RemoveInstance();
			if (_instanceIds != null && _instanceIds.Length != 0)
			{
				_counterInternal.InstanceName = CreateInstanceName(newInstanceName, _instanceIds);
			}
			else
			{
				_counterInternal.InstanceName = CreateInstanceName(newInstanceName);
			}
			_counterInternal.RawValue = rawValue;
			InstanceName = newInstanceName;
		}
	}

	public void RemoveInstance()
	{
		if (_counterInternal != null)
		{
			lock (_lockObj)
			{
				_counterInternal?.RemoveInstance();
			}
		}
	}

	public void RemoveInstanceAndDispose()
	{
		if (_counterInternal == null)
		{
			return;
		}
		lock (_lockObj)
		{
			if (_counterInternal != null)
			{
				_counterInternal.RemoveInstance();
				Dispose();
			}
		}
	}

	public void Close()
	{
		if (_counterInternal != null)
		{
			lock (_lockObj)
			{
				_counterInternal?.Close();
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _counterInternal != null)
		{
			lock (_lockObj)
			{
				_counterInternal.Dispose();
				_counterInternal = null;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
