namespace VideoOS.EventServer.Communication.Configuration;

public class AddSuppressStatusItemsRequestObject : RequestObject
{
	private SuppressStatusItem[] _suppressStatusItems;

	public SuppressStatusItem[] SuppressStatusItems => _suppressStatusItems;

	public AddSuppressStatusItemsRequestObject(int requestId, SuppressStatusItem[] suppressStatusItems)
		: base(requestId)
	{
		_suppressStatusItems = suppressStatusItems;
	}
}
