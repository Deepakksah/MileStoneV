using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("HostingServer")]
public abstract class HostingServer : VmoObjectIntBased
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

	public Uri InternalAddress
	{
		get
		{
			return new Uri(GetPropertyValue<string>("InternalAddress"));
		}
		set
		{
			string value2 = null;
			if (value != null)
			{
				value2 = value.ToString();
			}
			SetPropertyValue("InternalAddress", value2);
		}
	}

	public Uri ExternalAddress
	{
		get
		{
			return new Uri(GetPropertyValue<string>("ExternalAddress"));
		}
		set
		{
			string value2 = null;
			if (value != null)
			{
				value2 = value.ToString();
			}
			SetPropertyValue("ExternalAddress", value2);
		}
	}

	public HostingServer(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}
}
