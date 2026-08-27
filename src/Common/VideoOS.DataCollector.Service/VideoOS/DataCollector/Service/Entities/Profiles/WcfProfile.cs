using AutoMapper;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.DataCollector.Service.Entities.Profiles;

public class WcfProfile : Profile
{
	public WcfProfile()
	{
		CreateMap<VideoOS.WebService.Common.DataCollectorService.PerformanceCounterData, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterData>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterData, VideoOS.WebService.Common.DataCollectorService.PerformanceCounterData>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.PerformanceCounterValue, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue, VideoOS.WebService.Common.DataCollectorService.PerformanceCounterValue>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.CounterStateChangeData, VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateChangeData>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateChangeData, VideoOS.WebService.Common.DataCollectorService.CounterStateChangeData>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.CounterStateRulesConfiguration, VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRulesConfiguration>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRulesConfiguration, VideoOS.WebService.Common.DataCollectorService.CounterStateRulesConfiguration>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.CounterStateRuleConfiguration, VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRuleConfiguration>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRuleConfiguration, VideoOS.WebService.Common.DataCollectorService.CounterStateRuleConfiguration>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.DataCollectorConfiguration, VideoOS.DataCollector.Service.Entities.DataCollection.DataCollectorConfiguration>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.DataCollectorConfiguration, VideoOS.WebService.Common.DataCollectorService.DataCollectorConfiguration>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.PerformanceCounterConfiguration, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterConfiguration>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterConfiguration, VideoOS.WebService.Common.DataCollectorService.PerformanceCounterConfiguration>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.PerformanceCounterInstance, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance, VideoOS.WebService.Common.DataCollectorService.PerformanceCounterInstance>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.HistoricSampleRequestData, VideoOS.DataCollector.Service.Entities.DataCollection.HistoricSampleRequestData>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.HistoricSampleRequestData, VideoOS.WebService.Common.DataCollectorService.HistoricSampleRequestData>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.HistoricSample, VideoOS.DataCollector.Service.Entities.DataCollection.HistoricSample>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.HistoricSample, VideoOS.WebService.Common.DataCollectorService.HistoricSample>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.ServiceApplication, VideoOS.DataCollector.Service.Entities.DataCollection.ServiceApplication>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.ServiceApplication, VideoOS.WebService.Common.DataCollectorService.ServiceApplication>();
		CreateMap<VideoOS.WebService.Common.DataCollectorService.MachineProperty, VideoOS.DataCollector.Service.Entities.DataCollection.MachineProperty>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.MachineProperty, VideoOS.WebService.Common.DataCollectorService.MachineProperty>();
	}
}
