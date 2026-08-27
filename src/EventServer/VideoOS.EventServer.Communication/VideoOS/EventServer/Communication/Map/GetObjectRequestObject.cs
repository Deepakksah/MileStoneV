namespace VideoOS.EventServer.Communication.Map;

public class GetObjectRequestObject : RequestObject
{
	private string _id;

	private string _visualOwnerId;

	public string Id => _id;

	public string VisualOwnerId => _visualOwnerId;

	public GetObjectRequestObject(int requestId, string id, string visualOwnerId)
		: base(requestId)
	{
		_id = id;
		_visualOwnerId = visualOwnerId;
	}
}
