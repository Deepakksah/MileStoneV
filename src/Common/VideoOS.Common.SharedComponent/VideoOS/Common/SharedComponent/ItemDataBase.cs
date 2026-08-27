using System;
using VideoOS.Common.SharedComponent.Data;

namespace VideoOS.Common.SharedComponent;

public abstract class ItemDataBase
{
	protected PropertyBase _propertyBase;

	public PropertyBase Item => _propertyBase;

	public Guid Id
	{
		get
		{
			return _propertyBase.Id;
		}
		set
		{
			_propertyBase.Id = value;
		}
	}

	public string Name
	{
		get
		{
			return _propertyBase.DisplayName;
		}
		set
		{
			_propertyBase.DisplayName = value;
		}
	}

	public abstract string PartialXML { get; set; }
}
