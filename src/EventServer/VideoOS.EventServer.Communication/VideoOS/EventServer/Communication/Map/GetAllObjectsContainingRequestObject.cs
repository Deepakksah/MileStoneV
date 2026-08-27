using System;

namespace VideoOS.EventServer.Communication.Map;

public class GetAllObjectsContainingRequestObject : RequestObject
{
	private Guid _type;

	private string[] _ids;

	public Guid Type => _type;

	public string[] Ids => _ids;

	public GetAllObjectsContainingRequestObject(int requestId, Guid type, string[] ids)
		: base(requestId)
	{
		_type = type;
		_ids = ids;
	}
}
