using System;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Internal.Util;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlPluginCategory", Namespace = "urn:milestone-systems")]
public class AccessControlPluginCategory : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public Guid PluginId { get; private set; }

	[DataMember(Order = 2)]
	public string ExternalId { get; private set; }

	[DataMember(Order = 3)]
	public string Name { get; private set; }

	[DataMember(Order = 4)]
	public string AccessControlTypeString { get; private set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlPluginCategory(PluginCategory plugInCategory)
	{
		PluginId = plugInCategory.AccessControlPluginId;
		ExternalId = plugInCategory.ExternalId;
		Name = plugInCategory.Name;
		AccessControlTypeString = AccessControlTypeToStringConverter.TypeToString(plugInCategory.AccessControlType);
	}

	public Type GetAccessControlType()
	{
		return AccessControlTypeToStringConverter.TypeFromString(AccessControlTypeString);
	}

	public override string ToString()
	{
		return Name;
	}

	public override bool Equals(object obj)
	{
		if (obj is AccessControlPluginCategory { PluginId: var pluginId } accessControlPluginCategory)
		{
			if (pluginId.Equals(PluginId))
			{
				return accessControlPluginCategory.ExternalId.Equals(ExternalId);
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (PluginId.ToString() + ExternalId).GetHashCode();
	}
}
