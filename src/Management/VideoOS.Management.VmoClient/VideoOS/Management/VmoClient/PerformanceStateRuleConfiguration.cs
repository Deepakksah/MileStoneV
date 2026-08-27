using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("PerformanceStateRuleConfiguration")]
public class PerformanceStateRuleConfiguration : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public double CriticalThreshold
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<double>("CriticalThreshold");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("CriticalThreshold", value);
		}
	}

	public double WarningThreshold
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<double>("WarningThreshold");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("WarningThreshold", value);
		}
	}

	public int TimeWindow
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<int>("TimeWindow");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("TimeWindow", value);
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

	public int RuleType
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<int>("RuleType");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("RuleType", value);
		}
	}

	public bool Direction
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<bool>("Direction");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("Direction", value);
		}
	}

	public bool Disabled
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<bool>("Disabled");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("Disabled", value);
		}
	}

	public long Unit
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<long>("Unit");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("Unit", value);
		}
	}

	public PerformanceStateRuleConfiguration(string id, IVmoRepositories repositories)
		: this(string.Empty, id, new VmoPropertyCollection(), repositories, isCreated: true)
	{
	}

	public PerformanceStateRuleConfiguration(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public PerformanceStateRuleConfiguration(string internalId, string id, VmoPropertyCollection objectProperties, IVmoRepositories repositories, bool isCreated)
		: base(internalId, objectProperties, new VmoRelationCollection(), repositories, isCreated)
	{
		base.ObjectProperties.SetPropertyValue("Id", id);
	}

	public PerformanceStateRuleConfiguration(ManagementServer managementServer, Guid id)
		: this(id.ToString(), id.ToString(), new VmoPropertyCollection(), managementServer.Repositories, isCreated: true)
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
