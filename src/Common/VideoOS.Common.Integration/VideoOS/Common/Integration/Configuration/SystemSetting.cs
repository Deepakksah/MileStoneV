using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Common.Integration.Configuration;

public class SystemSetting
{
	private ConfigurationManager _manager;

	private ConfigurationDataSet.SystemSettingsRow _datarow;

	public ConfigurationDataSet.SystemSettingsRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Name;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Name = value;
		}
	}

	public string Value
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Setting;
		}
		[DebuggerStepThrough]
		set
		{
			if (_datarow.Setting != value)
			{
				_datarow.Setting = value;
			}
		}
	}

	public bool ValueAsBool
	{
		[DebuggerStepThrough]
		get
		{
			return Convert.ToBoolean(_datarow.Setting, CultureInfo.InvariantCulture);
		}
	}

	public double ValueAsDouble
	{
		[DebuggerStepThrough]
		get
		{
			return Convert.ToDouble(_datarow.Setting, CultureInfo.InvariantCulture);
		}
	}

	public int ValueAsInt32
	{
		[DebuggerStepThrough]
		get
		{
			return Convert.ToInt32(_datarow.Setting, CultureInfo.InvariantCulture);
		}
	}

	public long ValueAsInt64
	{
		[DebuggerStepThrough]
		get
		{
			return Convert.ToInt64(_datarow.Setting, CultureInfo.InvariantCulture);
		}
	}

	public string DisplaySortKey => Name;

	public SystemSetting(ConfigurationManager manager, ConfigurationDataSet.SystemSettingsRow row)
	{
		_manager = manager;
		_datarow = row;
	}
}
