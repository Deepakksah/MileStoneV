using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DebuggerDisplay("{DisplayName}")]
[DataContract(Name = "SecurityAction", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class SecurityAction
{
	[DataMember]
	public string ActionId { get; set; }

	[DataMember]
	public string DisplayName { get; set; }

	public SecurityAction()
	{
	}

	public SecurityAction(string actionId, string displayName)
	{
		ActionId = actionId;
		DisplayName = displayName;
	}
}
