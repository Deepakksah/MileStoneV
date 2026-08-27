using System;

namespace VideoOS.EventServer.Communication.Status;

public class GetStatusForAllObjectsOfTypeRequestObject : RequestObject
{
	private string _id;

	private Guid _type;

	private Guid _relationType;

	private int _maxLevels;

	public string Id => _id;

	public Guid Type => _type;

	public Guid RelationType => _relationType;

	public int MaxLevels => _maxLevels;

	public GetStatusForAllObjectsOfTypeRequestObject(int requestId, string id, Guid type, Guid relationType, int maxLevels)
		: base(requestId)
	{
		_id = id;
		_type = type;
		_relationType = relationType;
		_maxLevels = maxLevels;
	}
}
