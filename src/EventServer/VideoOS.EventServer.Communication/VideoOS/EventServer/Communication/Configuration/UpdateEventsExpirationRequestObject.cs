namespace VideoOS.EventServer.Communication.Configuration;

public class UpdateEventsExpirationRequestObject : RequestObject
{
	private UpdateEventsExpirationParams _updateEventsExpirationParams;

	public UpdateEventsExpirationParams UpdateEventsExpirationParams => _updateEventsExpirationParams;

	public UpdateEventsExpirationRequestObject(int requestId, UpdateEventsExpirationParams updateEventsExpirationParams)
		: base(requestId)
	{
		_updateEventsExpirationParams = updateEventsExpirationParams;
	}
}
