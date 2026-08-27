namespace VideoOS.EventServer.Communication.Configuration;

public class GetAlarmPropertiesRequestObject : RequestObject
{
	private string _key;

	public string Key => _key;

	public GetAlarmPropertiesRequestObject(int requestId, string key)
		: base(requestId)
	{
		_key = key;
	}
}
