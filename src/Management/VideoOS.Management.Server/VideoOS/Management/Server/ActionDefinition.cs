using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DebuggerDisplay("{DisplayName}")]
[DataContract(Name = "ActionDefinition", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class ActionDefinition
{
	[DataMember]
	public string NamespaceId { get; set; }

	public Guid NamespaceGuid
	{
		get
		{
			if (Guid.TryParse(NamespaceId, out var result))
			{
				return result;
			}
			return Guid.Empty;
		}
	}

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string DisplayName { get; set; }

	[DataMember]
	public bool Visible { get; set; }

	public ulong PermissionBit { get; set; }
}
