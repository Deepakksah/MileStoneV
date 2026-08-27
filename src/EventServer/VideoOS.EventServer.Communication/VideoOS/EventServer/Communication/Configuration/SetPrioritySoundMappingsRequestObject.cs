using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class SetPrioritySoundMappingsRequestObject : RequestObject
{
	private Tuple<Guid, Guid>[] _mappings;

	public Tuple<Guid, Guid>[] Mappings => _mappings;

	public SetPrioritySoundMappingsRequestObject(int requestId, Tuple<Guid, Guid>[] mappings)
		: base(requestId)
	{
		_mappings = mappings;
	}
}
