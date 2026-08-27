using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("HostedHardware")]
public abstract class HostedHardware : VmoObjectIntBased
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

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public string UserName
	{
		get
		{
			return GetPropertyValue<string>("UserName");
		}
		set
		{
			SetPropertyValue("UserName", value);
		}
	}

	public string Password
	{
		get
		{
			return GetPropertyValue<string>("Password");
		}
		set
		{
			SetPropertyValue("Password", value);
		}
	}

	protected string HostingInfo
	{
		get
		{
			return GetPropertyValue<string>("HostingInfo");
		}
		set
		{
			SetPropertyValue("HostingInfo", value);
		}
	}

	public string ManufacturerId
	{
		get
		{
			return GetPropertyValue<string>("ManufacturerId");
		}
		set
		{
			SetPropertyValue("ManufacturerId", value);
		}
	}

	public string CustomData
	{
		get
		{
			return GetPropertyValue<string>("CustomData");
		}
		set
		{
			SetPropertyValue("CustomData", value);
		}
	}

	public int HostingServerId
	{
		get
		{
			return GetPropertyValue<int>("HostingServerId");
		}
		set
		{
			SetPropertyValue("HostingServerId", value);
		}
	}

	public HostedHardware(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}
}
