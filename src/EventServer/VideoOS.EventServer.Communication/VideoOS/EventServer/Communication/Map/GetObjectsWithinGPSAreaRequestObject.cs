using VideoOS.EventServer.Communication.Map.Client;

namespace VideoOS.EventServer.Communication.Map;

public class GetObjectsWithinGPSAreaRequestObject : RequestObject
{
	private Bounds _area;

	public Bounds Area => _area;

	public GetObjectsWithinGPSAreaRequestObject(int requestId, Bounds area)
		: base(requestId)
	{
		_area = area;
	}
}
