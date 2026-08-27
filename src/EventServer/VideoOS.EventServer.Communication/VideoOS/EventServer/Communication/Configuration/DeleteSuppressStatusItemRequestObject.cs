using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class DeleteSuppressStatusItemRequestObject : RequestObject
{
	private Guid _sourceId;

	private Guid _suppressStatusItemId;

	public Guid SourceId => _sourceId;

	public Guid SuppressStatusItemId => _suppressStatusItemId;

	public DeleteSuppressStatusItemRequestObject(int requestId, Guid sourceId, Guid suppressStatusItemId)
		: base(requestId)
	{
		_sourceId = sourceId;
		_suppressStatusItemId = suppressStatusItemId;
	}
}
