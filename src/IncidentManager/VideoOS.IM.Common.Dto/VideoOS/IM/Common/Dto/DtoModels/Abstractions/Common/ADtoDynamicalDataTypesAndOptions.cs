using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.DynamicalDataTypesAndOptions;

namespace VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;

public abstract class ADtoDynamicalDataTypesAndOptions : ADtoBaseEntity, IDtoDynamicalDataTypesAndOptions, IDtoBaseEntity
{
	private int _ValueInt;

	private string _ValueStr;

	private decimal _ValueDecimal;

	private bool _ValueBool;

	private string _Name;

	private string _Title;

	private string _Description;

	private string _ClassOfData;

	private int _ClassOfDataint;

	private string _CustomAction;

	public int ValueInt
	{
		get
		{
			return _ValueInt;
		}
		set
		{
			if (_ValueInt != value)
			{
				_ValueInt = value;
				NotifyPropertyChanged("ValueInt");
			}
		}
	}

	public string ValueStr
	{
		get
		{
			return _ValueStr;
		}
		set
		{
			if (_ValueStr != value)
			{
				_ValueStr = value;
				NotifyPropertyChanged("ValueStr");
			}
		}
	}

	public decimal ValueDecimal
	{
		get
		{
			return _ValueDecimal;
		}
		set
		{
			if (_ValueDecimal != value)
			{
				_ValueDecimal = value;
				NotifyPropertyChanged("ValueDecimal");
			}
		}
	}

	public bool ValueBool
	{
		get
		{
			return _ValueBool;
		}
		set
		{
			if (_ValueBool != value)
			{
				_ValueBool = value;
				NotifyPropertyChanged("ValueBool");
			}
		}
	}

	public string Name
	{
		get
		{
			return _Name;
		}
		set
		{
			if (_Name != value)
			{
				_Name = value;
				NotifyPropertyChanged("Name");
			}
		}
	}

	public string Title
	{
		get
		{
			return _Title;
		}
		set
		{
			if (_Title != value)
			{
				_Title = value;
				NotifyPropertyChanged("Title");
			}
		}
	}

	public string Description
	{
		get
		{
			return _Description;
		}
		set
		{
			if (_Description != value)
			{
				_Description = value;
				NotifyPropertyChanged("Description");
			}
		}
	}

	public string ClassOfData
	{
		get
		{
			return _ClassOfData;
		}
		set
		{
			if (_ClassOfData != value)
			{
				_ClassOfData = value;
				NotifyPropertyChanged("ClassOfData");
			}
		}
	}

	public int ClassOfDataint
	{
		get
		{
			return _ClassOfDataint;
		}
		set
		{
			if (_ClassOfDataint != value)
			{
				_ClassOfDataint = value;
				NotifyPropertyChanged("ClassOfDataint");
			}
		}
	}

	public string CustomAction
	{
		get
		{
			return _CustomAction;
		}
		set
		{
			if (_CustomAction != value)
			{
				_CustomAction = value;
				NotifyPropertyChanged("CustomAction");
			}
		}
	}
}
