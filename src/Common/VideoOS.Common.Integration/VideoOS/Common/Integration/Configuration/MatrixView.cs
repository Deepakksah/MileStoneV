using System;
using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class MatrixView : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.MatrixRow _datarow;

	public DataRow DataRow => _datarow;

	public Guid ID
	{
		get
		{
			return new Guid(_datarow.IDMatrix);
		}
		set
		{
			_datarow.IDMatrix = value.ToString();
		}
	}

	public Guid IDMatrix
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.IDMatrix);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.IDMatrix = value.ToString();
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

	public string Description
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Description;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Description = value;
		}
	}

	public string Address
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Address;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Address = value;
		}
	}

	public int Port
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Port;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Port = value;
		}
	}

	public string Password
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Password;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Password = value;
		}
	}

	public string DisplaySortKey
	{
		[DebuggerStepThrough]
		get
		{
			return Name;
		}
	}

	public MatrixView(ConfigurationManager manager, ConfigurationDataSet.MatrixRow row)
	{
		_configurationManager = manager;
		_datarow = row;
	}
}
