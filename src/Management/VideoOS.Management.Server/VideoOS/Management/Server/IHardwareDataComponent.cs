using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IHardwareDataComponent : IDisposable
{
	IEnumerable<HardwareData> QueryHardwareByIds(Guid[] ids);

	IEnumerable<HardwareModifiedData> QueryHardwareChangedAfter(DateTime changesFrom);
}
