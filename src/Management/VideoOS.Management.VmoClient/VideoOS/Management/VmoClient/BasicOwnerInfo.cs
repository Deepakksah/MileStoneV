using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("BasicOwnerInfo")]
public class BasicOwnerInfo : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public string Name
	{
		get
		{
			return GetPropertyValue<string>("Name");
		}
		set
		{
			SetPropertyValue("Name", value);
		}
	}

	public string Property
	{
		get
		{
			return GetPropertyValue<string>("Property");
		}
		set
		{
			SetPropertyValue("Property", value);
		}
	}

	public string Order
	{
		get
		{
			return GetPropertyValue<string>("Order");
		}
		set
		{
			SetPropertyValue("Order", value);
		}
	}

	public BasicOwnerInfo(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public BasicOwnerInfo(ManagementServer managementServer, string name, string property, string order)
		: this(managementServer.Repositories, name, property, order)
	{
	}

	public BasicOwnerInfo(ManagementServer managementServer, Guid id, string name, string property, string order)
		: this(managementServer.Repositories, id, name, property, order)
	{
	}

	public BasicOwnerInfo(IVmoRepositories repositories, Guid id, string name, string property, string order)
		: this(repositories, name, property, order)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("The parameter cannot be an empty guid", "id");
		}
		SetPropertyValue("Id", id.ToString());
	}

	public BasicOwnerInfo(IVmoRepositories repositories, string name, string property, string order)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (order == null)
		{
			throw new ArgumentNullException("order");
		}
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Property argument is empty", "name");
		}
		if (string.IsNullOrEmpty(order))
		{
			throw new ArgumentException("Property argument is empty", "order");
		}
		SetPropertyValue("Name", name);
		SetPropertyValue("Property", property);
		SetPropertyValue("Order", order);
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
