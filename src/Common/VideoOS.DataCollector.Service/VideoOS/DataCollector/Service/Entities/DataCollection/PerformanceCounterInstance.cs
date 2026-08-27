using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public class PerformanceCounterInstance
{
	private string _instanceName;

	private string _path = "Unknown";

	private string _counterName;

	private string _categoryName;

	private Guid _instanceId = Guid.Empty;

	public string CategoryName
	{
		get
		{
			return _categoryName;
		}
		set
		{
			_categoryName = value;
			RecalculatePath();
		}
	}

	public string CounterName
	{
		get
		{
			return _counterName;
		}
		set
		{
			_counterName = value;
			RecalculatePath();
		}
	}

	public string InstanceName
	{
		get
		{
			return _instanceName;
		}
		set
		{
			_instanceName = value;
			_instanceId = (InstanceNameContainsId(_instanceName, out var id) ? id : Guid.Empty);
			RecalculatePath();
		}
	}

	public PerformanceCounterType CounterType { get; set; }

	public bool CounterDirection { get; set; }

	public bool EnableHistory { get; set; }

	public bool TriggerStatePerValue { get; set; }

	public PerformanceCounterCategoryType CategoryType { get; set; }

	public int SampleFrequency { get; set; }

	public string Path => _path;

	public PerformanceCounterInstance(string instanceName, string counterName, string categoryName)
	{
		InstanceName = instanceName;
		CounterName = counterName;
		CategoryName = categoryName;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(PerformanceCounterInstance? p1, PerformanceCounterInstance? p2)
	{
		if ((object)p1 == p2)
		{
			return true;
		}
		return p1?.Equals(p2) ?? false;
	}

	public static bool operator !=(PerformanceCounterInstance? p1, PerformanceCounterInstance? p2)
	{
		return !(p1 == p2);
	}

	public override bool Equals(object? obj)
	{
		PerformanceCounterInstance performanceCounterInstance = obj as PerformanceCounterInstance;
		if (performanceCounterInstance == null)
		{
			return false;
		}
		if (performanceCounterInstance.CategoryName == CategoryName && performanceCounterInstance.CounterName == CounterName)
		{
			return performanceCounterInstance.InstanceName == InstanceName;
		}
		return false;
	}

	public static bool InstanceNameContainsId(string path, out Guid id)
	{
		return Guid.TryParse(ExtractIdFromPath(path), out id);
	}

	private static string ExtractIdFromPath(string path)
	{
		int num = path.LastIndexOf('[');
		int num2 = path.LastIndexOf(']');
		if (num != -1 && num2 != -1 && num < num2)
		{
			return path.Substring(num + 1, num2 - num - 1);
		}
		return string.Empty;
	}

	private void RecalculatePath()
	{
		string path = "Unknown";
		if (!string.IsNullOrEmpty(CategoryName) && !string.IsNullOrEmpty(CounterName))
		{
			path = (string.IsNullOrEmpty(InstanceName) ? string.Format(CultureInfo.InvariantCulture, "\\{0}\\{1}", CategoryName, CounterName) : ((!(_instanceId != Guid.Empty)) ? string.Format(CultureInfo.InvariantCulture, "\\{0}({1})\\{2}", CategoryName, InstanceName, CounterName) : string.Format(CultureInfo.InvariantCulture, "\\{0}([{1}])\\{2}", CategoryName, _instanceId.ToString(), CounterName)));
		}
		_path = path;
	}
}
