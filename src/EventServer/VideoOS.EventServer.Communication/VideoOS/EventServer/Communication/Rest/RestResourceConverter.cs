using System;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.EventServer.Communication.Rest;

public class RestResourceConverter : IRestResourceConverter
{
	public readonly INameConvert NameConvert = new NameConvert();

	public const string RegisteredServicesName = "registeredServices";

	public RestResource GetRestResource(FQID deviceFqid)
	{
		if (null == deviceFqid)
		{
			return null;
		}
		string text;
		if (deviceFqid.Kind == Kind.Server)
		{
			switch (deviceFqid.ServerId?.ServerType)
			{
			case null:
				return Unknown(deviceFqid);
			case "XPCO":
				text = string.Empty;
				break;
			case "XPCORS":
				text = "RecordingServer";
				break;
			default:
				return Source("registeredServices", deviceFqid.ObjectId);
			}
		}
		else
		{
			text = Factory.GetItemTypeFromKind(deviceFqid.Kind);
		}
		if (text == null)
		{
			return Unknown(deviceFqid);
		}
		string text2 = null;
		try
		{
			text2 = NameConvert.ItemTypeToResourcePlural(text);
			if (text == "MIPItem")
			{
				string text3 = NameConvert.ItemTypeToResourcePlural("MIPKind");
				text2 = FormattableString.Invariant($"{text3}/{deviceFqid.Kind}/{text2}");
			}
		}
		catch (ResourceNotFoundException)
		{
		}
		if (text2 == null)
		{
			return Unknown(deviceFqid);
		}
		return Source(text2, deviceFqid.ObjectId);
	}

	public FQID GetFqid(RestResource restResource)
	{
		if (restResource == null)
		{
			return null;
		}
		Guid kindFromItemType = GetKindFromItemType((restResource.Name.Split('/').Length == 1) ? NameConvert.ResourcePluralToItemType(restResource.Name) : restResource.Name);
		Guid id = restResource.Id;
		FolderType folderType = ((!(id != Guid.Empty)) ? FolderType.SystemDefined : FolderType.No);
		return new FQID(VideoOS.Platform.Configuration.Instance.ServerFQID.ServerId, Guid.Empty, id, folderType, kindFromItemType);
	}

	public Guid GetKindFromRestResource(string restResource, out bool isFolder)
	{
		isFolder = false;
		if (string.IsNullOrEmpty(restResource))
		{
			return Guid.Empty;
		}
		Guid result = Guid.Empty;
		string[] array = restResource.Split('/');
		bool flag;
		if (array.Length == 2)
		{
			try
			{
				string text = array[0];
				if ("registeredServices".Equals(text, StringComparison.OrdinalIgnoreCase))
				{
					return Kind.Server;
				}
				if (text.EndsWith("Groups", StringComparison.InvariantCulture))
				{
					isFolder = true;
					text = text.Substring(0, text.Length - 6);
					text += "s";
				}
				string text2 = NameConvert.ResourcePluralToItemType(text);
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "System";
				}
				flag = Factory.TryGetKindFromItemType(text2, out result);
			}
			catch
			{
				flag = false;
			}
		}
		else
		{
			string text3 = NameConvert.ItemTypeToResourcePlural("MIPKind");
			string text4 = NameConvert.ItemTypeToResourcePlural("MIPItem");
			flag = Guid.TryParse(array[1], out result) && text3 == array[0] && text4 == array[2];
		}
		if (!flag)
		{
			return Guid.Empty;
		}
		return result;
	}

	protected static RestResource Unknown(FQID fqid)
	{
		return Source(fqid.Kind.ToString(), fqid.ObjectId);
	}

	protected static RestResource Source(string resourceName, Guid? sourceId)
	{
		if (!sourceId.HasValue)
		{
			return null;
		}
		return new RestResource
		{
			Name = resourceName,
			Id = sourceId.Value
		};
	}

	private static Guid GetKindFromItemType(string itemType)
	{
		if (Factory.TryGetKindFromItemType(itemType, out var kind))
		{
			return kind;
		}
		if (itemType.IndexOf("MIPItem", StringComparison.OrdinalIgnoreCase) > 0 || itemType.IndexOf("MIPKind", StringComparison.OrdinalIgnoreCase) > 0)
		{
			return Guid.Parse(itemType.Replace("MIPItem", string.Empty, StringComparison.OrdinalIgnoreCase).Replace("MIPKind", string.Empty, StringComparison.OrdinalIgnoreCase).Replace("s", string.Empty, StringComparison.OrdinalIgnoreCase)
				.Replace("/", string.Empty));
		}
		return Guid.Empty;
	}
}
