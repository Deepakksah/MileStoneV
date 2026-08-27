using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public interface IMetadataDevice : IStreamDevice, IDevice
{
	string Id { get; }

	string HostName { get; }

	IDictionary<Guid, MetadataType> MetadataTypes { get; }

	int Channel { get; }

	MetadataSecurity MetadataAccess { get; }

	SequenceType[] GetSequenceTypes();

	SequenceCollection GetSequenceAtTimeMinutes(DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType);
}
