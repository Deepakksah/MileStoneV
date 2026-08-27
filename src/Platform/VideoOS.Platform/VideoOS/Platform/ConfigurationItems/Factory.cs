using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;
using System.ServiceModel;
using System.Text.RegularExpressions;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.UI;

namespace VideoOS.Platform.ConfigurationItems;

public static class Factory
{
	public static Dictionary<Guid, string> KindToItemType = new Dictionary<Guid, string>
	{
		{
			Kind.AccessControlSystem,
			"AccessControlSystem"
		},
		{
			Kind.AccessControlUnit,
			"AccessControlUnit"
		},
		{
			Kind.Camera,
			"Camera"
		},
		{
			Kind.Folder,
			"Folder"
		},
		{
			Kind.Hardware,
			"Hardware"
		},
		{
			Kind.InputEvent,
			"InputEvent"
		},
		{
			Kind.Layout,
			"Layout"
		},
		{
			Kind.LayoutGroup,
			"LayoutGroup"
		},
		{
			Kind.Matrix,
			"Matrix"
		},
		{
			Kind.Metadata,
			"Metadata"
		},
		{
			Kind.Microphone,
			"Microphone"
		},
		{
			Kind.Output,
			"Output"
		},
		{
			Kind.Preset,
			"PtzPreset"
		},
		{
			Kind.Role,
			"Role"
		},
		{
			Kind.Server,
			"Server"
		},
		{
			Kind.SmartWall,
			"VideoWall"
		},
		{
			Kind.SmartWallMonitor,
			"Monitor"
		},
		{
			Kind.SmartWallPreset,
			"VideoWallPreset"
		},
		{
			Kind.Speaker,
			"Speaker"
		},
		{
			Kind.TimeProfile,
			"TimeProfile"
		},
		{
			Kind.TriggerEvent,
			"UserDefinedEvent"
		},
		{
			Kind.User,
			"User"
		},
		{
			Kind.View,
			"View"
		}
	};

	public const string MipItemType = "MIPItem";

	public const string MipItemFolderType = "MIPItemFolder";

	public const string MipKindType = "MIPKind";

	private static Dictionary<string, bool> SupportedIdTypes = new Dictionary<string, bool>();

	public static bool TraceCommunication { get; set; }

	public static void Log(string method, string message)
	{
		EnvironmentManager.Instance.Log(error: false, "ConfigAPI:" + method, message);
	}

	public static void LogBefore(string method, string path)
	{
		EnvironmentManager.Instance.Log(error: false, "ConfigAPI (Before):" + method, path);
	}

	public static void LogBefore(string method, ConfigurationItem item)
	{
		EnvironmentManager.Instance.Log(error: false, "ConfigAPI (Before):" + method, item.DisplayName);
	}

	public static void LogAfter(string method, ConfigurationItem item)
	{
		string text = ((item == null) ? "NULL" : item.DisplayName);
		EnvironmentManager.Instance.Log(error: false, "ConfigAPI (After):" + method, text);
	}

	public static void LogAfter(string method, ConfigurationItem[] items)
	{
		string text = ((items == null || items.Length == 0) ? "No Items" : ("Items returned:" + items.Length.ToString(CultureInfo.InvariantCulture)));
		EnvironmentManager.Instance.Log(error: false, "ConfigAPI (After):" + method, text);
	}

	public static string CreatePath(string itemType)
	{
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		return string.Format(CultureInfo.InvariantCulture, "/{0}", itemType);
	}

	public static string CreatePath(string itemType, Guid id)
	{
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		if (itemType == "System")
		{
			if (id == Guid.Empty)
			{
				return "/";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", itemType, id);
		}
		if (id == Guid.Empty)
		{
			if (!itemType.EndsWith("Folder", StringComparison.Ordinal))
			{
				itemType += "Folder";
			}
			return string.Format(CultureInfo.InvariantCulture, "/{0}", itemType);
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", itemType, id);
	}

	public static string CreatePath(string itemType, string id)
	{
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (itemType == "System")
		{
			if (string.IsNullOrEmpty(id) || id == Guid.Empty.ToString())
			{
				return "/";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", itemType, id);
		}
		if (string.IsNullOrEmpty(id) || id == Guid.Empty.ToString())
		{
			if (!itemType.EndsWith("Folder", StringComparison.Ordinal))
			{
				itemType += "Folder";
			}
			return string.Format(CultureInfo.InvariantCulture, "/{0}", itemType);
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", itemType, id);
	}

	public static string CreateFolderPath(string itemType, Guid id, string folderItemType)
	{
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		if (folderItemType == null)
		{
			throw new ArgumentNullException("folderItemType");
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]/{2}", itemType, id, folderItemType);
	}

	public static string CreateFolderPath(string itemType, string id, string folderItemType)
	{
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (folderItemType == null)
		{
			throw new ArgumentNullException("folderItemType");
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]/{2}", itemType, id, folderItemType);
	}

	public static string CreatePath(FQID fqid)
	{
		if (fqid == null)
		{
			throw new ArgumentNullException("fqid");
		}
		if (fqid.Kind == Kind.Server && fqid.ServerId.ServerType == "XPCO" && (fqid.ObjectId == Guid.Empty || fqid.ObjectId == fqid.ServerId.Id))
		{
			return "/";
		}
		string id = fqid.ObjectId.ToString().ToUpper(CultureInfo.InvariantCulture);
		if (fqid.Kind == Kind.Server && fqid.ServerId.ServerType == "XPCORS")
		{
			return CreatePath("RecordingServer", id);
		}
		string itemTypeFromKind = GetItemTypeFromKind(fqid.Kind);
		if (!string.IsNullOrEmpty(itemTypeFromKind))
		{
			return CreatePath(itemTypeFromKind, id);
		}
		return null;
	}

	public static string GetItemTypeFromKind(Guid kind)
	{
		if (KindToItemType.ContainsKey(kind))
		{
			return KindToItemType[kind];
		}
		if (!VideoOS.Platform.UI.Util.IsBuiltInKind(kind))
		{
			return "MIPItem";
		}
		return null;
	}

	public static bool TryGetKindFromItemType(string itemType, out Guid kind)
	{
		if (itemType == "BasicUser")
		{
			kind = Kind.User;
			return true;
		}
		if (itemType == "RecordingServer")
		{
			kind = Kind.Server;
			return true;
		}
		foreach (KeyValuePair<Guid, string> item in KindToItemType)
		{
			if (item.Value == itemType)
			{
				kind = item.Key;
				return true;
			}
		}
		kind = Guid.Empty;
		return false;
	}

	public static string GetIdFromPath(string path)
	{
		if (path == null)
		{
			return null;
		}
		Match match = new Regex(".*\\[(.*)\\].*").Match(path);
		if (match.Success && match.Groups.Count == 2)
		{
			return match.Groups[1].Value;
		}
		return null;
	}

	public static Item ConvertMIPItemToItem(ConfigurationItem configItem)
	{
		if (configItem == null)
		{
			throw new ArgumentNullException("configItem");
		}
		configItem.Properties.TryGetPropertyValue("Name", out var value);
		configItem.Properties.TryGetPropertyValue("Id", out var value2);
		configItem.Properties.TryGetPropertyValue("FQID.Kind", out var value3);
		configItem.Properties.TryGetPropertyValue("FQID.ParentId", out var value4);
		configItem.Properties.TryGetPropertyValue("FQID.ServerId", out var value5);
		if (!configItem.Properties.TryGetPropertyValue("FQID.Scheme", out var value6))
		{
			value6 = Uri.UriSchemeHttp;
		}
		configItem.Properties.TryGetPropertyValue("FQID.Host", out var value7);
		configItem.Properties.TryGetPropertyValue("FQID.Port", out var value8);
		configItem.Properties.TryGetPropertyValue("FQID.Type", out var value9);
		configItem.Properties.TryGetPropertyValue("GisPoint", out var value10);
		int.TryParse(value8, out var result);
		Guid.TryParse(value5, out var result2);
		Guid.TryParse(value4, out var result3);
		Guid.TryParse(value2, out var result4);
		Guid.TryParse(value3, out var result5);
		Item item = new Item(new FQID(new ServerId(value9, value7, result, result2), result3, result4, FolderType.No, result5), value);
		if (configItem.EnableProperty != null)
		{
			item.Enabled = configItem.EnableProperty.Enabled;
		}
		if (!string.IsNullOrEmpty(value10))
		{
			item.PositioningInformation = new PositioningInformation(value10, 0.0, 0.0, 0.0);
		}
		Property[] properties = configItem.Properties;
		foreach (Property property in properties)
		{
			if (property.Key != "Name" && property.Key != "Id" && property.Key != "GisPoint" && !property.Key.StartsWith("FQID.", StringComparison.Ordinal))
			{
				item.Properties[property.Key] = property.Value;
			}
		}
		return item;
	}

	public static Item ConvertMIPItemToItem(MIPItem mipItem)
	{
		if (mipItem == null)
		{
			throw new ArgumentNullException("mipItem");
		}
		string value = mipItem.Properties.GetValue("Name");
		string value2 = mipItem.Properties.GetValue("Id");
		string value3 = mipItem.Properties.GetValue("FQID.Kind");
		string value4 = mipItem.Properties.GetValue("FQID.ParentId");
		string value5 = mipItem.Properties.GetValue("FQID.ServerId");
		string text = mipItem.Properties.GetValue("FQID.Scheme");
		if (text == null)
		{
			text = Uri.UriSchemeHttp;
		}
		string value6 = mipItem.Properties.GetValue("FQID.Host");
		string value7 = mipItem.Properties.GetValue("FQID.Port");
		string value8 = mipItem.Properties.GetValue("FQID.Type");
		string value9 = mipItem.Properties.GetValue("GisPoint");
		int.TryParse(value7, out var result);
		Guid.TryParse(value5, out var result2);
		Guid.TryParse(value4, out var result3);
		Guid.TryParse(value2, out var result4);
		Guid.TryParse(value3, out var result5);
		Item item = new Item(new FQID(new ServerId(value8, text, value6, result, result2), result3, result4, FolderType.No, result5), value);
		item.Enabled = mipItem.Enabled;
		if (!string.IsNullOrEmpty(value9))
		{
			item.PositioningInformation = new PositioningInformation(value9, 0.0, 0.0, 0.0);
		}
		foreach (string key in mipItem.Properties.Keys)
		{
			if (key != "Name" && key != "Id" && key != "GisPoint" && !key.StartsWith("FQID.", StringComparison.Ordinal))
			{
				item.Properties[key] = mipItem.Properties.GetValue(key);
			}
		}
		return item;
	}

	public static void FillMIPItemInItem(MIPItem mipItem, Item item)
	{
		if (mipItem == null)
		{
			throw new ArgumentNullException("mipItem");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		string value = mipItem.Properties.GetValue("Name");
		string value2 = mipItem.Properties.GetValue("Id");
		string value3 = mipItem.Properties.GetValue("FQID.Kind");
		string value4 = mipItem.Properties.GetValue("FQID.ParentId");
		string value5 = mipItem.Properties.GetValue("FQID.ServerId");
		string scheme = mipItem.Properties.GetValue("FQID.Scheme") ?? Uri.UriSchemeHttp;
		string value6 = mipItem.Properties.GetValue("FQID.Host");
		string value7 = mipItem.Properties.GetValue("FQID.Port");
		string value8 = mipItem.Properties.GetValue("FQID.Type");
		string value9 = mipItem.Properties.GetValue("GisPoint");
		int.TryParse(value7, out var result);
		Guid.TryParse(value5, out var result2);
		Guid.TryParse(value4, out var result3);
		Guid.TryParse(value2, out var result4);
		Guid.TryParse(value3, out var result5);
		item.FQID = new FQID(new ServerId(value8, scheme, value6, result, result2), result3, result4, FolderType.No, result5);
		item.Name = value;
		item.Enabled = mipItem.Enabled;
		if (!string.IsNullOrEmpty(value9))
		{
			item.PositioningInformation = new PositioningInformation(value9, 0.0, 0.0, 0.0);
		}
		foreach (string key in mipItem.Properties.Keys)
		{
			if (key != "Name" && key != "Id" && key != "GisPoint" && !key.StartsWith("FQID.", StringComparison.Ordinal))
			{
				item.Properties[key] = mipItem.Properties.GetValue(key);
			}
		}
	}

	public static bool IsConfigurationAPISupported(ServerId serverId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		return serverId.ServerType == "XPCO";
	}

	public static bool IsLoginEstablished(ServerId serverId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		LoginSettings loginSettings = ((serverId.UserContext != null) ? serverId.UserContext.GetLoginSettings(serverId) : LoginSettingsCache.GetLoginSettings(serverId));
		if (loginSettings != null)
		{
			if (string.IsNullOrEmpty(loginSettings.Token))
			{
				return serverId.IsExportType;
			}
			return true;
		}
		return false;
	}

	public static bool IsItemTypeSupported(ServerId serverId, string itemType)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		if (serverId.ServerType != "XPCO")
		{
			return false;
		}
		if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient && EnvironmentManager.Instance.SmartClientInOfflineMode)
		{
			return false;
		}
		if (!IsLoginEstablished(serverId))
		{
			return true;
		}
		string key = serverId.Id.ToString() + itemType;
		if (SupportedIdTypes.ContainsKey(key))
		{
			return SupportedIdTypes[key];
		}
		string path = (itemType.EndsWith("Folder", StringComparison.Ordinal) ? CreatePath(itemType) : CreatePath(itemType, Guid.Empty));
		try
		{
			new ConfigurationApiItemWrapper(serverId, path);
			SupportedIdTypes[key] = true;
		}
		catch (FaultException<PathNotFoundMIPException>)
		{
			SupportedIdTypes[key] = true;
		}
		catch (FaultException<ArgumentExceptionFault>)
		{
			SupportedIdTypes[key] = false;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("Factory.IsItemTypeSupported", exception);
			return false;
		}
		return SupportedIdTypes[key];
	}

	public static IConfigurationItem GetConfigurationItem(FQID fqid)
	{
		if (fqid == null)
		{
			throw new ArgumentNullException("fqid");
		}
		string itemTypeFromKind = GetItemTypeFromKind(fqid.Kind);
		string typeName = "VideoOS.Platform.ConfigurationItems." + itemTypeFromKind;
		try
		{
			ObjectHandle objectHandle = Activator.CreateInstanceFrom(Assembly.GetExecutingAssembly().Location, typeName, ignoreCase: false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[1] { fqid }, null, null);
			if (objectHandle != null)
			{
				return objectHandle.Unwrap() as IConfigurationItem;
			}
		}
		catch (Exception innerException)
		{
			throw new MIPException("Unable to create class", innerException);
		}
		return null;
	}
}
