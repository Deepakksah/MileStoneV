using System;
using System.Collections.Generic;
using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.DynamicalDataTypesAndOptions;

public class DtoDynamicalDataTypesAndOptions : ADtoDynamicalDataTypesAndOptions
{
	private string _typeOfItemStr;

	private Type _typeOfItem;

	private int? _numberOfForeignReferences;

	private string _entityScopeStr;

	private string _propertyScopeStr;

	private List<DtoIncident> _usedInIncidentAsType;

	private List<DtoIncident> _usedInIncidentAsStatus;

	private List<DtoIncident> _usedInIncidentAsCategory1;

	private List<DtoIncident> _usedInIncidentAsCategory2;

	private List<DtoIncident> _usedInIncidentAsCategory3;

	private List<DtoIncident> _usedInIncidentAsCategory4;

	private List<DtoIncident> _usedInIncidentAsCategory5;

	public string TypeOfItemStr
	{
		get
		{
			return _typeOfItemStr;
		}
		set
		{
			if (_typeOfItemStr != value)
			{
				_typeOfItemStr = value;
				NotifyPropertyChanged("TypeOfItemStr");
			}
		}
	}

	public Type TypeOfItem
	{
		get
		{
			return _typeOfItem;
		}
		set
		{
			if (_typeOfItem != value)
			{
				_typeOfItem = value;
				NotifyPropertyChanged("TypeOfItem");
			}
		}
	}

	public int? NumberOfForeignReferences
	{
		get
		{
			return _numberOfForeignReferences;
		}
		set
		{
			if (_numberOfForeignReferences != value)
			{
				_numberOfForeignReferences = value;
				NotifyPropertyChanged("NumberOfForeignReferences");
			}
		}
	}

	public string EntityScopeStr
	{
		get
		{
			return _entityScopeStr;
		}
		set
		{
			if (_entityScopeStr != value)
			{
				_entityScopeStr = value;
				NotifyPropertyChanged("EntityScopeStr");
			}
		}
	}

	public string PropertyScopeStr
	{
		get
		{
			return _propertyScopeStr;
		}
		set
		{
			if (_propertyScopeStr != value)
			{
				_propertyScopeStr = value;
				NotifyPropertyChanged("PropertyScopeStr");
			}
		}
	}

	public List<DtoIncident> UsedInIncident_AsType
	{
		get
		{
			return _usedInIncidentAsType;
		}
		set
		{
			if (_usedInIncidentAsType != value)
			{
				_usedInIncidentAsType = value;
				NotifyPropertyChanged("UsedInIncident_AsType");
			}
		}
	}

	public List<DtoIncident> UsedInIncident_AsStatus
	{
		get
		{
			return _usedInIncidentAsStatus;
		}
		set
		{
			if (_usedInIncidentAsStatus != value)
			{
				_usedInIncidentAsStatus = value;
				NotifyPropertyChanged("UsedInIncident_AsStatus");
			}
		}
	}

	public List<DtoIncident> UsedInIncident_AsCategory1
	{
		get
		{
			return _usedInIncidentAsCategory1;
		}
		set
		{
			if (_usedInIncidentAsCategory1 != value)
			{
				_usedInIncidentAsCategory1 = value;
				NotifyPropertyChanged("UsedInIncident_AsCategory1");
			}
		}
	}

	public List<DtoIncident> UsedInIncident_AsCategory2
	{
		get
		{
			return _usedInIncidentAsCategory2;
		}
		set
		{
			if (_usedInIncidentAsCategory2 != value)
			{
				_usedInIncidentAsCategory2 = value;
				NotifyPropertyChanged("UsedInIncident_AsCategory2");
			}
		}
	}

	public List<DtoIncident> UsedInIncident_AsCategory3
	{
		get
		{
			return _usedInIncidentAsCategory3;
		}
		set
		{
			if (_usedInIncidentAsCategory3 != value)
			{
				_usedInIncidentAsCategory3 = value;
				NotifyPropertyChanged("UsedInIncident_AsCategory3");
			}
		}
	}

	public List<DtoIncident> UsedInIncident_AsCategory4
	{
		get
		{
			return _usedInIncidentAsCategory4;
		}
		set
		{
			if (_usedInIncidentAsCategory4 != value)
			{
				_usedInIncidentAsCategory4 = value;
				NotifyPropertyChanged("UsedInIncident_AsCategory4");
			}
		}
	}

	public List<DtoIncident> UsedInIncident_AsCategory5
	{
		get
		{
			return _usedInIncidentAsCategory5;
		}
		set
		{
			if (_usedInIncidentAsCategory5 != value)
			{
				_usedInIncidentAsCategory5 = value;
				NotifyPropertyChanged("UsedInIncident_AsCategory5");
			}
		}
	}

	public override string ToString()
	{
		return base.Title;
	}
}
