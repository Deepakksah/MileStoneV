using System;

namespace VideoOS.EventServer.Communication.Map;

public class ActivatePlugInActionRequestObject : RequestObject
{
	private Guid _typeId;

	private string _itemId;

	private string _actionId;

	public Guid TypeId => _typeId;

	public string ItemId => _itemId;

	public string ActionId => _actionId;

	public ActivatePlugInActionRequestObject(int requestId, Guid typeId, string itemId, string actionId)
		: base(requestId)
	{
		_typeId = typeId;
		_itemId = itemId;
		_actionId = actionId;
	}
}
