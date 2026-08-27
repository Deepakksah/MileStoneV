namespace VideoOS.EventServer.Communication.Status;

public class GetStatusRequestObject : RequestObject
{
	private string _id;

	public string Id => _id;

	public GetStatusRequestObject(int requestId, string id)
		: base(requestId)
	{
		_id = id;
	}
}
