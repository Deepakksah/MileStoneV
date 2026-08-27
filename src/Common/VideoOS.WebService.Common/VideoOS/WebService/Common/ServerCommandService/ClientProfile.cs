using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("ClientProfile: {Name} ({Id})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ClientProfile
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	[DataMember]
	public DateTime SettingsLastModified { get; set; }

	[DataMember]
	public ClientProfileSetting[] ClientProfileSettings { get; set; }
}
