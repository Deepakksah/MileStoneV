using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class EndPointIdentityData
{
	public FQID EndPointFQID;

	public string IdentityName;
}
