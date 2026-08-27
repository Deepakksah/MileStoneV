namespace VideoOS.EventServer.Communication.Configuration;

public class ModifyDisableAlarmItemRequestObject : RequestObject
{
	private DisableAlarmItem _disableAlarmItem;

	public DisableAlarmItem DisableAlarmItem => _disableAlarmItem;

	public ModifyDisableAlarmItemRequestObject(int requestId, DisableAlarmItem disableAlarmItem)
		: base(requestId)
	{
		_disableAlarmItem = disableAlarmItem;
	}
}
