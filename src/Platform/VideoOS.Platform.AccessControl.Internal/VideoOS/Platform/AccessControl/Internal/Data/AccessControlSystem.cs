using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlSystem", Namespace = "urn:milestone-systems")]
public class AccessControlSystem : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public Guid Id { get; set; }

	[DataMember(Order = 2)]
	public string Name { get; set; }

	[DataMember(Order = 3)]
	public bool CredentialHolderSearchSupported { get; set; }

	[DataMember(Order = 4)]
	public bool CredentialHolderImageOverrideEnabled { get; set; }

	[DataMember(Order = 5)]
	public AccessControlExternalCommand[] AccessControlExternalCommands { get; set; }

	[DataMember(Order = 6)]
	public bool PersonalizedLoginSupported { get; set; }

	[DataMember(Order = 7)]
	public bool PersonalizedLoginEnabled { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlSystem()
	{
	}

	public AccessControlSystem(Guid id, string name)
	{
		Id = id;
		Name = name;
	}
}
