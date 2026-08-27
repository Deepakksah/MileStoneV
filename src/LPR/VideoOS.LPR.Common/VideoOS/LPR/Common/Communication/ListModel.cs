using System;
using System.Collections.Generic;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.Communication;

public class ListModel
{
	private string _listName;

	private Guid _listId;

	private Guid _siteId;

	private IList<LicensePlatePropertyDefinition> _propertyDefinitions;

	public string ListName
	{
		get
		{
			return _listName;
		}
		set
		{
			_listName = value;
		}
	}

	public Guid ListId
	{
		get
		{
			return _listId;
		}
		set
		{
			_listId = value;
		}
	}

	public Guid SiteId
	{
		get
		{
			return _siteId;
		}
		set
		{
			_siteId = value;
		}
	}

	public IList<LicensePlatePropertyDefinition> PropertyDefinition
	{
		get
		{
			return _propertyDefinitions;
		}
		set
		{
			_propertyDefinitions = value;
		}
	}
}
