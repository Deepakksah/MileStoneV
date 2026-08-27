namespace VideoOS.EventServer.Communication.Map;

public class GetAllObjectIDsOnVisualElementRequestObject : RequestObject
{
	private string _id;

	public string Id => _id;

	public GetAllObjectIDsOnVisualElementRequestObject(int requestId, string id)
		: base(requestId)
	{
		_id = id;
	}
}
