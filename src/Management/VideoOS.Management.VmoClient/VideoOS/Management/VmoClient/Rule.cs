using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("Rule")]
public class Rule : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
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

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public string RuleXml
	{
		get
		{
			return GetPropertyValue<string>("RuleXml");
		}
		set
		{
			SetPropertyValue("RuleXml", value);
		}
	}

	public bool Enabled
	{
		get
		{
			return GetPropertyValue<bool>("Enabled");
		}
		set
		{
			SetPropertyValue("Enabled", value);
		}
	}

	public override bool IsCustomPropertiesSupported => false;

	public Rule(ManagementServer managementServer, string name, string description, string ruleXml, bool enabled)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), managementServer.Repositories, isInitialized: true)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (description == null)
		{
			throw new ArgumentNullException("description");
		}
		if (ruleXml == null)
		{
			throw new ArgumentNullException("ruleXml");
		}
		SetPropertyValue("Name", name);
		SetPropertyValue("Description", description);
		SetPropertyValue("RuleXml", ruleXml);
		SetPropertyValue("Enabled", enabled);
	}

	public Rule(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(InternalId);
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
