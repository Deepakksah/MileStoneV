namespace VideoOS.EventServer.Communication.Configuration;

public class ModifySuppressStatusItemRequestObject : RequestObject
{
	private SuppressStatusItem _suppressStatusItem;

	public SuppressStatusItem SuppressStatusItem => _suppressStatusItem;

	public ModifySuppressStatusItemRequestObject(int requestId, SuppressStatusItem suppressStatusItem)
		: base(requestId)
	{
		_suppressStatusItem = suppressStatusItem;
	}
}
