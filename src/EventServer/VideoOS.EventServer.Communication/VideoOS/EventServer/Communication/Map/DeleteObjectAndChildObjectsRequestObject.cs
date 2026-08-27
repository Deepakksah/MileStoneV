namespace VideoOS.EventServer.Communication.Map;

public class DeleteObjectAndChildObjectsRequestObject : RequestObject
{
	private string _id;

	private string _visualOwnerId;

	private bool _recursive;

	public string Id => _id;

	public string VisualOwnerId => _visualOwnerId;

	public bool Recursive => _recursive;

	public DeleteObjectAndChildObjectsRequestObject(int requestId, string id, string visualOwnerId, bool recursive)
		: base(requestId)
	{
		_id = id;
		_visualOwnerId = visualOwnerId;
		_recursive = recursive;
	}
}
