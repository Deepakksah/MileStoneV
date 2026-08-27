namespace VideoOS.EventServer.Communication;

public class RequestObject
{
	private int _requestId;

	public int RequestId => _requestId;

	public RequestObject(int requestId)
	{
		_requestId = requestId;
	}
}
