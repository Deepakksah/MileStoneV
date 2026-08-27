using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IRecorderDataComponent : IDisposable
{
	IEnumerable<RecorderData> QueryEnabledRecorders(Guid[] recorderIds);

	RecorderData QueryActiveRecorder(Guid recorderId);

	IEnumerable<RecorderModifiedData> QueryRecordersChangedAfter(DateTime changesFrom);
}
