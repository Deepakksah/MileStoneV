using System;

namespace VideoOS.EventServer.Communication.Map;

public class GetIconDataRequestObject : RequestObject
{
	private Guid _type;

	private Guid _iconId;

	public Guid Type => _type;

	public Guid IconId => _iconId;

	public GetIconDataRequestObject(int requestId, Guid type, Guid iconId)
		: base(requestId)
	{
		_type = type;
		_iconId = iconId;
	}
}
