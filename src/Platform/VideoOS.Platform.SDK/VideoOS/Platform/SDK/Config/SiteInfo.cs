using System;
using System.Collections.ObjectModel;
using VideoOS.Platform.SDK.Platform;

namespace VideoOS.Platform.SDK.Config;

public class SiteInfo
{
	private string _productName = "";

	private string _serverVersion = "";

	public string Name { get; set; }

	public Guid Id { get; set; }

	public string ServerSubType { get; set; }

	public string ServerType { get; set; }

	public string Description { get; set; }

	public int ProductCode { get; set; }

	public string ProductName
	{
		get
		{
			if (_productName == "")
			{
				_productName = "Milestone XProtect Corporate";
				FindSystemInfo()?.Properties.TryGetValue("ProductName", out _productName);
			}
			return _productName;
		}
		set
		{
			_productName = value;
		}
	}

	public DateTime LastModified { get; set; }

	public string ConnectionState { get; set; }

	public Collection<Address> AddressCollection { get; set; }

	public string ServerVersion
	{
		get
		{
			if (_serverVersion == "")
			{
				FindSystemInfo()?.Properties.TryGetValue("ProductVersion", out _serverVersion);
			}
			return _serverVersion;
		}
		set
		{
			_serverVersion = value;
		}
	}

	public string ServiceAccount { get; set; }

	public int SyncStatus { get; set; }

	public bool IndividualLoadSupport { get; set; }

	public Guid Parent { get; set; }

	public bool ParentEncrypted { get; set; }

	public Collection<Guid> Children { get; set; }

	public Uri ServerUri
	{
		get
		{
			string text = (ParentEncrypted ? "Internal" : "Server");
			foreach (Address item in AddressCollection)
			{
				if (item.Type == text)
				{
					return new Uri(item.Url);
				}
			}
			return new Uri("http://localhost:80");
		}
	}

	public SiteInfo()
	{
		Name = "";
		ServerSubType = "";
		ServerType = "XP";
		Description = "";
		ConnectionState = "Attached";
		AddressCollection = new Collection<Address>();
		ServerVersion = "";
		ServiceAccount = "";
		Children = new Collection<Guid>();
		SyncStatus = 0;
		ProductCode = 0;
	}

	private ISystemInfo FindSystemInfo()
	{
		if (Configuration.Instance is SDKConfiguration)
		{
			ServerId serverId = new ServerId(ServerType, "", 80, Id);
			return ((SDKConfiguration)Configuration.Instance).FindSystemInfo(serverId, populate: true);
		}
		return null;
	}
}
