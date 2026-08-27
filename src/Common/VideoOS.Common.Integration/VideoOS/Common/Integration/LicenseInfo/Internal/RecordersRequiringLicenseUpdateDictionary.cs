using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

[Serializable]
public sealed class RecordersRequiringLicenseUpdateDictionary : Dictionary<Guid, byte[]>, IRecordersRequiringLicenseUpdateDictionary, IDictionary<Guid, byte[]>, ICollection<KeyValuePair<Guid, byte[]>>, IEnumerable<KeyValuePair<Guid, byte[]>>, IEnumerable
{
	public RecordersRequiringLicenseUpdateDictionary()
	{
	}

	private RecordersRequiringLicenseUpdateDictionary(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
