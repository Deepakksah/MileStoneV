using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("RuleSource")]
public class RuleSource : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public int DataCollectorId
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<int>("DataCollectorId");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("DataCollectorId", value);
		}
	}

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

	public string CounterRuleId
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<string>("CounterRuleId");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("CounterRuleId", value);
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

	public bool Configurable
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<bool>("Configurable");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("Configurable", value);
		}
	}

	public int CounterId
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<int>("CounterId");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("CounterId", value);
		}
	}

	public string CounterPath
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<string>("CounterPath");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("CounterPath", value);
		}
	}

	public string ParentId
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<string>("ParentId");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("ParentId", value);
		}
	}

	public RuleSource(string id, IVmoRepositories repositories)
		: this(string.Empty, id, new VmoPropertyCollection(), repositories, isCreated: true)
	{
	}

	public RuleSource(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public RuleSource(string internalId, string id, VmoPropertyCollection objectProperties, IVmoRepositories repositories, bool isCreated)
		: base(internalId, objectProperties, new VmoRelationCollection(), repositories, isCreated)
	{
		base.ObjectProperties.SetPropertyValue("Id", id);
	}

	public RuleSource(ManagementServer managementServer, Guid id)
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
