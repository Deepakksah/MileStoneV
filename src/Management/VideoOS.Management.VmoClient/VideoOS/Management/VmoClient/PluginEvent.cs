using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("PluginEvent")]
public abstract class PluginEvent : VmoObjectGuidBased
{
	public virtual DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public virtual string Name
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

	public virtual string Definition
	{
		get
		{
			return GetPropertyValue<string>("Definition");
		}
		set
		{
			SetPropertyValue("Definition", value);
		}
	}

	public PluginEvent(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
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

	public void Trigger(Guid sourceId)
	{
		Trigger(sourceId, null, null, null);
	}

	public void Trigger(Guid sourceId, string customData)
	{
		Trigger(sourceId, null, null, customData);
	}

	public void Trigger(Guid sourceId, Guid[] metadataDeviceIds, string customData)
	{
		Trigger(sourceId, metadataDeviceIds, null, customData);
	}

	public void Trigger(Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData)
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.TriggerPluginEvent(new Guid(base.InternalId), sourceId, metadataDeviceIds, metadataDeviceGroupIds, customData);
	}
}
