using VideoOS.IM.Common.Service.Properties;

namespace VideoOS.IM.Common.Service.Common;

public interface IDtoGlobalSettingsManager
{
	[CanBeNull]
	ManagerOptions ManagerOptionInstance { get; set; }
}
