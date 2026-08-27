using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.RecorderCommunication;

public interface IAttachedRecorders : IEnumerable<IRecorderEntry>, IEnumerable
{
	IRecorderEntry GetRecorderEntryFromUri(string channelUri);

	IRecorderEntry GetRecorderEntry(Guid serviceId);

	IEnumerable<IRecorderEntry> GetServices(Guid recorderId);

	bool TryGetRecorderEntry(Guid serviceId, out IRecorderEntry entry);

	void Register(IRecorderEntry entry);

	void UnRegisterByServiceId(Guid serviceId);

	void UnRegister(IRecorderEntry entry);

	RecorderStatusInfo GetRecorderStatus(Guid serviceId);

	RecorderStatusInfo[] GetRecorderStatus();

	RecorderCompatibilityInfo GetRecorderCompatibilityStatus(Guid serviceId);
}
