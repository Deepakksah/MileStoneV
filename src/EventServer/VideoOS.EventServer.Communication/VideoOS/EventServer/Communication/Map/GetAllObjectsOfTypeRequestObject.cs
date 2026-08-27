using System;

namespace VideoOS.EventServer.Communication.Map;

public class GetAllObjectsOfTypeRequestObject : RequestObject
{
	private Guid _type;

	private Guid _relationType;

	private string _id;

	private int _maxLevels;

	private bool _includeRoot;

	public Guid Type => _type;

	public Guid RelationType => _relationType;

	public string Id => _id;

	public int MaxLevels => _maxLevels;

	public bool IncludeRoot => _includeRoot;

	public GetAllObjectsOfTypeRequestObject(int requestId, Guid type, Guid relationType, string id, int maxLevels, bool includeRoot)
		: base(requestId)
	{
		_type = type;
		_relationType = relationType;
		_id = id;
		_maxLevels = maxLevels;
		_includeRoot = includeRoot;
	}
}
