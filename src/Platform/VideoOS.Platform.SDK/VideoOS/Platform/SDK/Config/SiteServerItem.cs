using System;
using System.Collections.Generic;

namespace VideoOS.Platform.SDK.Config;

public class SiteServerItem : Item
{
	private SiteInfo _siteInfo;

	private SiteInfoManager _siteInfoManager;

	public SiteServerItem(ServerId serverId, SiteInfo siteInfo, SiteInfoManager siteInfoManager)
		: base(serverId, siteInfo.Parent, siteInfo.Id, siteInfo.Name, FolderType.SystemDefined, Kind.Server)
	{
		_siteInfo = siteInfo;
		_siteInfoManager = siteInfoManager;
		int num = 0;
		foreach (Address item in siteInfo.AddressCollection)
		{
			if (item.Type == "Server")
			{
				base.Properties["URL_Server"] = item.Url.ToString();
				base.Properties["URL"] = item.Url.ToString();
			}
			base.Properties["Address"] = item.Url.ToString();
			base.Properties["Address" + num] = item.Url.ToString();
			num++;
		}
		HasChildren = ((siteInfo.Children.Count == 0) ? HasChildren.No : HasChildren.Yes);
		HasRelated = HasRelated.No;
		base.Properties["ServerType"] = siteInfo.ServerSubType;
		base.Properties["Description"] = siteInfo.Description;
		base.Properties["ProductName"] = siteInfo.ProductName;
		base.Properties["LastModified"] = ToDateTimeString(siteInfo.LastModified);
		base.Properties["ConnectionState"] = siteInfo.ConnectionState;
		base.Properties["ServerVersion"] = siteInfo.ServerVersion;
		base.Properties["ServiceAccount"] = siteInfo.ServiceAccount;
		base.Properties["SyncStatus"] = siteInfo.SyncStatus.ToString();
		base.Properties["ProductCode"] = siteInfo.ProductCode.ToString();
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		if (_siteInfo.Children != null)
		{
			foreach (Guid child in _siteInfo.Children)
			{
				SiteInfo site = _siteInfoManager.GetSite(child);
				if (site != null)
				{
					list.Add(new SiteServerItem(new ServerId(site.ServerType, site.ServerUri.Scheme, site.ServerUri.DnsSafeHost, site.ServerUri.Port, site.Id), site, _siteInfoManager));
				}
			}
		}
		return list;
	}

	private static string ToDateTimeString(DateTime dateTime)
	{
		if (dateTime == DateTime.MinValue)
		{
			return "";
		}
		try
		{
			return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
		}
		catch (ArgumentOutOfRangeException)
		{
		}
		return string.Empty;
	}
}
