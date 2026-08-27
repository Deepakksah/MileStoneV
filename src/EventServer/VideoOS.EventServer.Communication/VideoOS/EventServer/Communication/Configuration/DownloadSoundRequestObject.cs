using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class DownloadSoundRequestObject : RequestObject
{
	private Guid _id;

	public Guid Id => _id;

	public DownloadSoundRequestObject(int requestId, Guid id)
		: base(requestId)
	{
		_id = id;
	}
}
