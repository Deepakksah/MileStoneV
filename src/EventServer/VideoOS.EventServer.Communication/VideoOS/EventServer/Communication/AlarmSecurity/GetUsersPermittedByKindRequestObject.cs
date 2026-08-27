using System;

namespace VideoOS.EventServer.Communication.AlarmSecurity;

public class GetUsersPermittedByKindRequestObject : RequestObject
{
	private Guid _type;

	public Guid Type => _type;

	public GetUsersPermittedByKindRequestObject(int requestId, Guid type)
		: base(requestId)
	{
		_type = type;
	}
}
