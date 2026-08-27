using VideoOS.IM.Server.Service.Annotations;

namespace VideoOS.IM.Server.Service.OEM;

public interface IOemResourceService
{
	[NotNull]
	string IncidentManagerApplicationName { get; }

	[NotNull]
	string CompanyNameShort { get; }

	[NotNull]
	string CommonApplicationDataFolder { get; }

	[NotNull]
	string IncidentManagerApplicationNameLong { get; }
}
