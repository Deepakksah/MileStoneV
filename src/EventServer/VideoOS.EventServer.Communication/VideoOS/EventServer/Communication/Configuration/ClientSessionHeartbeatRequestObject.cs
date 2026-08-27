namespace VideoOS.EventServer.Communication.Configuration;

public class ClientSessionHeartbeatRequestObject : RequestObject
{
	private HeartbeatParams _heartbeatParams;

	public HeartbeatParams HeartbeatParams => _heartbeatParams;

	public ClientSessionHeartbeatRequestObject(int requestId, HeartbeatParams heartbeatParams)
		: base(requestId)
	{
		_heartbeatParams = heartbeatParams;
	}
}
