using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "AccessControlEntry", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
[DebuggerDisplay("AccessControlEntry. {Sid,nq},  {ActionId,nq},  {AccessDeny ? \"DENY\" : \"ALLOW\",nq}")]
public class AccessControlEntry
{
	[DataMember]
	public string Sid { get; set; }

	[DataMember]
	public string ActionId { get; set; }

	[DataMember]
	public bool AccessDeny { get; set; }

	public AccessControlEntry()
	{
	}

	public AccessControlEntry(string actionId, string sid, bool accessDeny)
	{
		ActionId = actionId;
		Sid = sid;
		AccessDeny = accessDeny;
	}
}
