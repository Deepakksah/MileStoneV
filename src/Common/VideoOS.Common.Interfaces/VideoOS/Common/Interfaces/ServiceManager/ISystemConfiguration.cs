using System.Collections.Generic;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Interfaces.ServiceManager;

public interface ISystemConfiguration
{
	int WebsitePort { get; set; }

	IEnumerable<IHardwareInfo> FetchHardware();
}
