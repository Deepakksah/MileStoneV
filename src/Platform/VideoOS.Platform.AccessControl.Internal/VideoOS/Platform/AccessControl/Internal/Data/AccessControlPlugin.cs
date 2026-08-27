using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlPlugin", Namespace = "urn:milestone-systems")]
public class AccessControlPlugin : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public Guid Id { get; private set; }

	[DataMember(Order = 2)]
	public string Name { get; private set; }

	[DataMember(Order = 3)]
	public Version Version { get; private set; }

	[DataMember(Order = 4)]
	public string VersionText { get; private set; }

	[DataMember(Order = 5)]
	public AccessControlPropertyDefinition[] PropertyDefinitions { get; private set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlPlugin()
	{
	}

	public AccessControlPlugin(Guid id, string name, Version version, string versionText, AccessControlPropertyDefinition[] propertyDefinitions)
	{
		Id = id;
		Name = name;
		Version = version;
		VersionText = versionText;
		PropertyDefinitions = propertyDefinitions;
	}
}
