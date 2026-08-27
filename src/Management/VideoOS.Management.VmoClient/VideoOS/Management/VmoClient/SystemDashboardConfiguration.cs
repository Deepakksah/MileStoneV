using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("SystemDashboardConfiguration")]
public class SystemDashboardConfiguration : VmoObjectIntBased, ICreatable, IUpdatable, IDeletable
{
	public int DashboardId
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<int>("DashboardId");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("DashboardId", value);
		}
	}

	public IList<int> EventTypes
	{
		get
		{
			string propertyValue = base.ObjectProperties.GetPropertyValue<string>("EventTypes");
			if (propertyValue == null)
			{
				return null;
			}
			string[] array = propertyValue.Split(';');
			List<int> list = new List<int>();
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result))
				{
					list.Add(result);
				}
			}
			return list;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			string text = string.Empty;
			foreach (int item in value)
			{
				text = text + item + ";";
			}
			base.ObjectProperties.SetPropertyValue("EventTypes", text);
		}
	}

	public string StateSourceType
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<string>("StateSourceType");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("StateSourceType", value);
		}
	}

	public string Name
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<string>("Name");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("Name", value);
		}
	}

	public string SingleStateSourceId
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<string>("SingleStateSourceId");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("SingleStateSourceId", value);
		}
	}

	public int Position
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<int>("Position");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("Position", value);
		}
	}

	public SystemDashboardConfiguration(string internalId, int dashboardId, IVmoRepositories repositories)
		: this(internalId, dashboardId, new VmoPropertyCollection(), repositories, isCreated: true)
	{
	}

	public SystemDashboardConfiguration(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public SystemDashboardConfiguration(string internalId, int dashboardId, VmoPropertyCollection objectProperties, IVmoRepositories repositories, bool isCreated)
		: base(internalId, objectProperties, new VmoRelationCollection(), repositories, isCreated)
	{
		if (dashboardId < 1)
		{
			throw new ArgumentException("The parameter should be positive", "dashboardId");
		}
		base.ObjectProperties.SetPropertyValue("DashboardId", dashboardId);
		base.ObjectProperties.SetPropertyValue("Id", dashboardId);
	}

	public SystemDashboardConfiguration(ManagementServer managementServer, Guid id, int dashboardId)
		: this(id.ToString(), dashboardId, new VmoPropertyCollection(), managementServer.Repositories, isCreated: true)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("The parameter cannot be an empty guid", "id");
		}
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}
}
