using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("TimeProfile")]
public abstract class TimeProfile : VmoObjectGuidBased
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

	public string Description
	{
		get
		{
			return GetPropertyValue<string>("Description");
		}
		set
		{
			SetPropertyValue("Description", value);
		}
	}

	public string DefinitionXml
	{
		get
		{
			return GetPropertyValue<string>("DefinitionXml");
		}
		set
		{
			SetPropertyValue("DefinitionXml", value);
		}
	}

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public TimeProfile(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
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
}
