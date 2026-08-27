using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterServiceApplications;

public interface IRegisterInstalledServicesService
{
	ServiceApplication[] EnumServiceApplications(ref ServiceApplication[] applicationList);
}
