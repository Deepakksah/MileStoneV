using System;
using System.Collections.Generic;

namespace VideoOS.Server.MIP.Communication.Client.EventConverter;

public sealed class ParseMetadataResult
{
	public IEnumerable<Guid> ReferenceIds { get; private set; }

	public IReadOnlyDictionary<string, string> OtherMetadata { get; private set; }

	public ParseMetadataResult(IEnumerable<Guid> referenceIds, IReadOnlyDictionary<string, string> otherMetadata)
	{
		ReferenceIds = referenceIds;
		OtherMetadata = otherMetadata;
	}
}
