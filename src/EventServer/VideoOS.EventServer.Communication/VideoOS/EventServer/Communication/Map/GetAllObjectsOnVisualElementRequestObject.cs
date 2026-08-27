namespace VideoOS.EventServer.Communication.Map;

public class GetAllObjectsOnVisualElementRequestObject : RequestObject
{
	private string _id;

	public string Id => _id;

	public GetAllObjectsOnVisualElementRequestObject(int requestId, string id)
		: base(requestId)
	{
		_id = id;
	}
}
