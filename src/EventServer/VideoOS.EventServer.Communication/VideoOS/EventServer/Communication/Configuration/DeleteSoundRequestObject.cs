using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class DeleteSoundRequestObject : RequestObject
{
	private Guid _id;

	public Guid Id => _id;

	public DeleteSoundRequestObject(int requestId, Guid id)
		: base(requestId)
	{
		_id = id;
	}
}
