namespace VideoOS.EventServer.Communication.Configuration;

public class AddDisableAlarmItemsRequestObject : RequestObject
{
	private DisableAlarmItem[] _disableAlarmItems;

	public DisableAlarmItem[] DisableAlarmItems => _disableAlarmItems;

	public AddDisableAlarmItemsRequestObject(int requestId, DisableAlarmItem[] disableAlarmItems)
		: base(requestId)
	{
		_disableAlarmItems = disableAlarmItems;
	}
}
