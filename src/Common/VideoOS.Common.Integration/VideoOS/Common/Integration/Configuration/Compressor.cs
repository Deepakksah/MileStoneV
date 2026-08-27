using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Common.Integration.Configuration;

public class Compressor
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.CompressorsRow _datarow;

	public ConfigurationDataSet.CompressorsRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public Guid IDCompressor
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.IDCodec);
		}
	}

	public int Handler
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Handler;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Handler = value;
		}
	}

	public int Flags
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Flags;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Flags = value;
		}
	}

	public int Quality
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Quality;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Quality = value;
		}
	}

	public int KeyFrames
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.KeyFrames;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.KeyFrames = value;
		}
	}

	public int DataRate
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.DataRate;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.DataRate = value;
		}
	}

	public byte[] ConfigData
	{
		get
		{
			if (!_datarow.IsConfigDataNull())
			{
				return _datarow.ConfigData;
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				_datarow.SetConfigDataNull();
			}
			else
			{
				_datarow.ConfigData = value;
			}
		}
	}

	public string DisplaySortKey => Handler.ToString(CultureInfo.InvariantCulture);

	public Compressor(ConfigurationManager manager, ConfigurationDataSet.CompressorsRow row)
	{
		_configurationManager = manager;
		_datarow = row;
	}
}
