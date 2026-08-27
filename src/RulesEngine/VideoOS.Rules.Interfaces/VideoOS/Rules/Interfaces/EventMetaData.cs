using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Rules.Interfaces;

public sealed class EventMetaData
{
	public enum MetaTypeEnum
	{
		Identifier = 1,
		Data
	}

	public static class MetaTypeString
	{
		public const string Identifier = "identifier";

		public const string Data = "data";
	}

	public enum DataTypeEnum
	{
		Unknown,
		Bool,
		Int,
		Double,
		String,
		Guid
	}

	public static class DataTypeString
	{
		public const string Bool = "bool";

		public const string Int = "int";

		public const string Double = "double";

		public const string String = "string";

		public const string Guid = "guid";
	}

	private string _name;

	private MetaTypeEnum _metaType;

	private DataTypeEnum _dataType;

	private string _value;

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return _name;
		}
	}

	public MetaTypeEnum MetaType
	{
		[DebuggerStepThrough]
		get
		{
			return _metaType;
		}
		[DebuggerStepThrough]
		set
		{
			_metaType = value;
		}
	}

	public DataTypeEnum DataType
	{
		[DebuggerStepThrough]
		get
		{
			return _dataType;
		}
		[DebuggerStepThrough]
		set
		{
			_dataType = value;
		}
	}

	public string Value
	{
		[DebuggerStepThrough]
		get
		{
			return _value;
		}
	}

	public EventMetaData(string name, string metaType, string dataType, string value)
	{
		_name = name;
		_metaType = GetMetaTypeEnum(metaType);
		_dataType = GetDataTypeEnum(dataType);
		_value = value;
	}

	public EventMetaData(string name, MetaTypeEnum metaType, DataTypeEnum dataType, string value)
	{
		_name = name;
		_metaType = metaType;
		_dataType = dataType;
		_value = value;
	}

	public bool GetValueAsBool()
	{
		return string.Equals(_value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
	}

	public int GetValueAsInt()
	{
		return Convert.ToInt32(_value, CultureInfo.InvariantCulture);
	}

	public double GetValueAsDouble()
	{
		return Convert.ToDouble(_value, CultureInfo.InvariantCulture);
	}

	public string GetValueAsString()
	{
		return _value;
	}

	public Guid GetValueAsGuid()
	{
		return new Guid(_value);
	}

	public static string GetMetaTypeString(MetaTypeEnum metaType)
	{
		return metaType switch
		{
			MetaTypeEnum.Identifier => "identifier", 
			_ => "data", 
		};
	}

	public static MetaTypeEnum GetMetaTypeEnum(string metaType)
	{
		if (!(metaType == "identifier"))
		{
			if (!(metaType == "data"))
			{
			}
			return MetaTypeEnum.Data;
		}
		return MetaTypeEnum.Identifier;
	}

	public static string GetDataTypeString(DataTypeEnum dataType)
	{
		return dataType switch
		{
			DataTypeEnum.Bool => "bool", 
			DataTypeEnum.Int => "int", 
			DataTypeEnum.Double => "double", 
			DataTypeEnum.String => "string", 
			DataTypeEnum.Guid => "guid", 
			_ => "string", 
		};
	}

	public static DataTypeEnum GetDataTypeEnum(string dataType)
	{
		return dataType switch
		{
			"bool" => DataTypeEnum.Bool, 
			"int" => DataTypeEnum.Int, 
			"double" => DataTypeEnum.Double, 
			"string" => DataTypeEnum.String, 
			"guid" => DataTypeEnum.Guid, 
			_ => DataTypeEnum.String, 
		};
	}
}
