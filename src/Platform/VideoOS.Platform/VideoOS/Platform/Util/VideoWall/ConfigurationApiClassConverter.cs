using System;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.Util.VideoWall;

public class ConfigurationApiClassConverter
{
	public static FQID ConvertToFQID(ServerId serverId, ConfigurationItem configurationItem)
	{
		Guid kind = Guid.Empty;
		Guid result = Guid.Empty;
		Guid result2 = Guid.Empty;
		FolderType folderType = FolderType.No;
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(configurationItem.Path);
		ConfigurationItemPath configurationItemPath2 = new ConfigurationItemPath(configurationItem.ParentPath);
		switch (configurationItem.ItemType)
		{
		case "LayoutGroupFolder":
			throw new InternalMIPException("Should not be a MIP item", null);
		case "LayoutGroup":
			kind = Kind.LayoutGroup;
			Guid.TryParse(configurationItemPath.Id, out result2);
			break;
		case "LayoutFolder":
			throw new InternalMIPException("Should not be a MIP item", null);
		case "Layout":
			kind = Kind.Layout;
			Guid.TryParse(configurationItemPath2.Id, out result);
			Guid.TryParse(configurationItemPath.Id, out result2);
			break;
		case "VideoWall":
			kind = Kind.VideoWall;
			Guid.TryParse(configurationItemPath.Id, out result2);
			break;
		case "VideoWallFolder":
			throw new InternalMIPException("Should not be a MIP item", null);
		case "Monitor":
			kind = Kind.VideoWallMonitor;
			Guid.TryParse(configurationItemPath2.Id, out result);
			Guid.TryParse(configurationItemPath.Id, out result2);
			break;
		case "MonitorFolder":
			throw new InternalMIPException("Should not be a MIP item", null);
		case "VideoWallPreset":
			kind = Kind.VideoWallPreset;
			Guid.TryParse(configurationItemPath2.Id, out result);
			Guid.TryParse(configurationItemPath.Id, out result2);
			break;
		case "VideoWallPresetFolder":
			throw new InternalMIPException("Should not be a MIP item", null);
		case "MonitorPreset":
			kind = Kind.View;
			Guid.TryParse(configurationItemPath2.Id, out result);
			Guid.TryParse(configurationItemPath.Id, out result2);
			break;
		case "MonitorPresetFolder":
			throw new InternalMIPException("Should not be a MIP item", null);
		case "GisMapLocationFolder":
			throw new InternalMIPException("Should not be a MIP item", null);
		case "GisMapLocation":
			kind = Kind.GisMapLocation;
			Guid.TryParse(configurationItemPath.Id, out result2);
			break;
		}
		return new FQID(serverId, result, result2, folderType, kind);
	}

	public static string ConvertToItemType(Guid kind)
	{
		if (kind == Kind.LayoutGroup)
		{
			return "LayoutGroup";
		}
		if (kind == Kind.Layout)
		{
			return "Layout";
		}
		if (kind == Kind.VideoWall)
		{
			return "VideoWall";
		}
		if (kind == Kind.VideoWallMonitor)
		{
			return "Monitor";
		}
		if (kind == Kind.VideoWallPreset)
		{
			return "VideoWallPreset";
		}
		if (kind == Kind.Camera)
		{
			return "Camera";
		}
		if (kind == Kind.GisMapLocation)
		{
			return "GisMapLocation";
		}
		return "";
	}

	public static HasChildren HasChildren(Guid kind)
	{
		if (kind == Kind.VideoWall || kind == Kind.LayoutGroup)
		{
			return VideoOS.Platform.HasChildren.Yes;
		}
		return VideoOS.Platform.HasChildren.No;
	}
}
