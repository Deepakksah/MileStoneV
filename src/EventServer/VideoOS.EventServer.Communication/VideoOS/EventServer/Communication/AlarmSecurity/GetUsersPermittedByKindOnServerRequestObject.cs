using System;

namespace VideoOS.EventServer.Communication.AlarmSecurity;

public class GetUsersPermittedByKindOnServerRequestObject : RequestObject
{
	private Guid _type;

	private string[] _serverIds;

	public Guid Type => _type;

	public string[] ServerIds => _serverIds;

	public GetUsersPermittedByKindOnServerRequestObject(int requestId, Guid type, string[] serverIds)
		: base(requestId)
	{
		_type = type;
		_serverIds = serverIds;
	}
}
