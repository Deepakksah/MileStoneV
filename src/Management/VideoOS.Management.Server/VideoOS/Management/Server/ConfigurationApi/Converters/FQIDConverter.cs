using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;

namespace VideoOS.Management.Server.ConfigurationApi.Converters;

public class FQIDConverter : IFQIDConverter
{
	public string ConvertFQIDsToPaths(IEnumerable<FQID> fqids)
	{
		if (fqids == null)
		{
			return string.Empty;
		}
		return string.Join(",", from s in fqids.Select(ConvertFQIDToPath)
			where s != null
			select s);
	}

	public FQID[] ConvertPathsToFQIDs(IGeminiRequestContext requestContext, IEnumerable<ConfigurationItemPath> paths)
	{
		return paths.Select((ConfigurationItemPath s) => ConvertPathToFQID(requestContext, s, Guid.Empty)).ToArray();
	}

	public string ConvertFQIDToPath(FQID fqid)
	{
		string text = null;
		if (fqid != null)
		{
			text = Factory.CreatePath(fqid);
		}
		if (text == null)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			object arg;
			if ((object)fqid != null)
			{
				_ = fqid.ObjectId;
				arg = fqid.ObjectId.ToString();
			}
			else
			{
				arg = "NULL";
			}
			instance.Log(error: true, "FQIDConverter", string.Format(invariantCulture, "Unable to convert selected FQID: {0} to a path - selection will go away", arg));
		}
		return text;
	}

	public FQID ConvertPathToFQID(IGeminiRequestContext requestContext, ConfigurationItemPath path, Guid defaultMipKind)
	{
		string text = path.ItemType;
		Guid guid = (string.IsNullOrEmpty(path.Id) ? Guid.Empty : new Guid(path.Id));
		bool flag = false;
		if (text.EndsWith("Folder", StringComparison.Ordinal))
		{
			text = text.Substring(0, text.Length - 6);
			flag = true;
		}
		switch (text)
		{
		case "System":
			if (guid == Guid.Empty)
			{
				return new FQID(new ServerId(), Guid.Empty, guid, FolderType.SystemDefined, Kind.Server);
			}
			return new FQID(EnvironmentManager.Instance.MasterSite.ServerId, Guid.Empty, guid, FolderType.No, Kind.Server);
		case "RecordingServer":
			return new FQID(new ServerId("XPCORS", "not-used", 7563, guid), Guid.Empty, guid, FolderType.No, Kind.Server);
		case "Server":
			return new FQID(new ServerId("XP", "not-used", 0, guid), Guid.Empty, guid, FolderType.SystemDefined, Kind.Server);
		case "Hardware":
			return RecordingServerDeviceFqid(requestContext, text, guid, Kind.Hardware);
		case "Camera":
			return RecordingServerDeviceFqid(requestContext, text, guid, Kind.Camera);
		case "Microphone":
			return RecordingServerDeviceFqid(requestContext, text, guid, Kind.Microphone);
		case "Speaker":
			return RecordingServerDeviceFqid(requestContext, text, guid, Kind.Speaker);
		case "Metadata":
			return RecordingServerDeviceFqid(requestContext, text, guid, Kind.Metadata);
		case "InputEvent":
			return RecordingServerDeviceFqid(requestContext, text, guid, Kind.InputEvent);
		case "Output":
			return RecordingServerDeviceFqid(requestContext, text, guid, Kind.Output);
		case "UserDefinedEvent":
			if (flag)
			{
				return new FQID(EnvironmentManager.Instance.MasterSite.ServerId, EnvironmentManager.Instance.MasterSite.ServerId.Id, guid, FolderType.SystemDefined, Kind.TriggerEvent);
			}
			return new FQID(EnvironmentManager.Instance.MasterSite.ServerId, Guid.Empty, guid, FolderType.No, Kind.TriggerEvent);
		case "AnalyticsEvent":
			return new FQID(EnvironmentManager.Instance.MasterSite.ServerId, Guid.Empty, guid, FolderType.No, AnalyticsEventConfigurationApi.AnalyticsKind);
		case "MIPItem":
			if (flag && defaultMipKind != Guid.Empty)
			{
				return new FQID(new ServerId("XP", "", 0, Guid.Empty), Guid.Empty, guid, FolderType.SystemDefined, defaultMipKind);
			}
			return MIPItemFqid(requestContext, guid);
		case "GenericEvent":
			return GenericEventFqid(requestContext, guid);
		case "AccessControlSystem":
			if (flag)
			{
				return new FQID(new ServerId("XP", "", 0, Guid.Empty), Guid.Empty, Guid.Empty, FolderType.No, Kind.AccessControlSystem);
			}
			return new FQID(new ServerId("XP", "", 0, Guid.Empty), Guid.Empty, guid, FolderType.No, Kind.AccessControlSystem);
		case "AccessControlUnit":
			if (flag)
			{
				return new FQID(new ServerId("XP", "", 0, guid), guid, Guid.Empty, FolderType.SystemDefined, Kind.AccessControlUnit);
			}
			return new FQID(new ServerId("XP", "", 0, Guid.Empty), Guid.Empty, guid, FolderType.No, Kind.AccessControlUnit);
		case "AccessControlRuleGroup":
			return AccessControlRuleGroupFqid(requestContext, guid);
		default:
			EnvironmentManager.Instance.Log(error: false, "FQIDConverter", "Missing conversion of ItemType:" + path.ItemType);
			return null;
		}
	}

	private static FQID AccessControlRuleGroupFqid(IGeminiRequestContext requestContext, Guid id)
	{
		if (id == Guid.Empty)
		{
			return new FQID(new ServerId("XP", "", 0, Guid.Empty), Guid.Empty, id, FolderType.No, Kind.AccessControlDoor);
		}
		string itemIdFromServerGroupId = ServerGroup.GetItemIdFromServerGroupId(requestContext, id.ToString().ToUpperInvariant());
		if (string.IsNullOrEmpty(itemIdFromServerGroupId))
		{
			return null;
		}
		return new FQID(new ServerId("XP", "", 0, Kind.AccessControlUnit), Kind.AccessControlUnit, new Guid(itemIdFromServerGroupId), FolderType.No, Kind.AccessControlDoor);
	}

	private static FQID MIPItemFqid(IGeminiRequestContext requestContext, Guid id)
	{
		if (id != Guid.Empty)
		{
			Item item = MIPItem.GetItem(requestContext, id);
			if (item != null)
			{
				return item.FQID;
			}
		}
		return null;
	}

	private static FQID GenericEventFqid(IGeminiRequestContext requestContext, Guid id)
	{
		if (id != Guid.Empty)
		{
			Item item = MIPItem.GetItem(requestContext, id);
			if (item != null && item.Properties.TryGetValue("ShadowId", out var value))
			{
				return new FQID(item.FQID.ServerId, item.FQID.ParentId, new Guid(value), FolderType.No, Kind.TriggerEvent);
			}
		}
		return null;
	}

	private static FQID RecordingServerDeviceFqid(IGeminiRequestContext requestContext, string itemType, Guid id, Guid kind)
	{
		string text = null;
		if (id != Guid.Empty)
		{
			if (itemType == "Hardware")
			{
				Hardware hardware = Hardware.QueryHardwareById(requestContext, id.ToString());
				if (hardware == null)
				{
					Guid guid = id;
					throw new ArgumentException("Unable to lookup Hardware with id: " + guid.ToString());
				}
				text = hardware.RecordingServerId;
			}
			else
			{
				Device device = Device.QueryDeviceById(requestContext, id.ToString());
				if (device == null)
				{
					Guid guid = id;
					throw new ArgumentException("Unable to lookup Device with id: " + guid.ToString());
				}
				text = device.RecordingServerId;
			}
			RecordingServer recordingServer = RecordingServer.QueryRecordingServer(requestContext, text);
			Uri uri = new UriBuilder(recordingServer.WebServerUri).Uri;
			return new FQID(new ServerId("XPCORS", uri.AbsolutePath, uri.Port, new Guid(recordingServer.Id)), EnvironmentManager.Instance.MasterSite.ServerId.Id, id, FolderType.No, kind);
		}
		return new FQID(new ServerId("XPCORS", "", 7563, Guid.Empty), EnvironmentManager.Instance.MasterSite.ServerId.Id, id, FolderType.SystemDefined, kind);
	}
}
