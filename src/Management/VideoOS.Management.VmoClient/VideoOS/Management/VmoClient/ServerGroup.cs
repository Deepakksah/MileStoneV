using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ServerGroup")]
public abstract class ServerGroup : VmoObjectGuidBased
{
	public const string SmartClientGroupId = "430C2FE2-B233-42DA-B508-1A22EF03383D";

	public const string RecordingServerGroupId = "E0BE6096-A1D6-464e-B028-B45FF2BA28AF";

	public const string FailoverGroupId = "6A85FFC3-C134-4338-A072-024817CB5820";

	public const string MIPObjectGroupId = "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683";

	public const string DevicesGroupId = "9178D2CE-888E-4058-9992-C99D506BE63A";

	public const string ManagementServerId = "909749AE-88C4-4780-A381-383D691E98FA";

	public const string PrivateViewGroupId = "4BF8A8B0-54AB-4E82-994F-98D8E9584176";

	public const string ViewLayoutGroup4_3Id = "7EB5B9CC-D56F-4785-BA72-4E71FADA8C6B";

	public const string ViewLayoutGroup16_9Id = "14C3A703-6835-4843-B18C-8DB95436A80C";

	public const string ViewLayoutGroup4_3PortraitId = "E0199F86-4A16-4A1F-B655-8BE7B9DCA3E1";

	public const string ViewLayoutGroup16_9PortraitId = "9F4A3B56-9A30-44B5-9C19-5E7421164F96";

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

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public string ParentServerGroupId
	{
		get
		{
			return GetPropertyValue<string>("ParentServerGroupId");
		}
		set
		{
			SetPropertyValue("ParentServerGroupId", value);
		}
	}

	public ServerGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
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

	public override void AddNotification(VmoObject obj)
	{
		if (string.Equals(obj.ObjectProperties.GetPropertyValue<string>("ParentServerGroupId"), InternalId, StringComparison.OrdinalIgnoreCase))
		{
			base.AddNotification(obj);
		}
	}
}
