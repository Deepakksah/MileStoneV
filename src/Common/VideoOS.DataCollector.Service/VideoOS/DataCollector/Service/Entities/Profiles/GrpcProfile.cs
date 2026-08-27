using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Grpc;

namespace VideoOS.DataCollector.Service.Entities.Profiles;

public class GrpcProfile : Profile
{
	private class GetPerformanceCounterValuesResponseConverter : ITypeConverter<IEnumerable<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue>, GetPerformanceCounterValuesResponse>
	{
		public GetPerformanceCounterValuesResponse Convert(IEnumerable<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue> source, GetPerformanceCounterValuesResponse destination, ResolutionContext context)
		{
			GetPerformanceCounterValuesResponse getPerformanceCounterValuesResponse = new GetPerformanceCounterValuesResponse();
			getPerformanceCounterValuesResponse.Values.AddRange(context.Mapper.Map<IEnumerable<VideoOS.DataCollector.Service.Grpc.PerformanceCounterValue>>(source));
			return getPerformanceCounterValuesResponse;
		}
	}

	public GrpcProfile()
	{
		CreateMap<VideoOS.DataCollector.Service.Grpc.PerformanceCounterValue, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue>().ForMember((VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue dest) => dest.CollectionTime, delegate(IMemberConfigurationExpression<VideoOS.DataCollector.Service.Grpc.PerformanceCounterValue, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue, DateTime> opt)
		{
			opt.MapFrom((VideoOS.DataCollector.Service.Grpc.PerformanceCounterValue src) => src.CollectionTime.ToDateTime());
		});
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue, VideoOS.DataCollector.Service.Grpc.PerformanceCounterValue>().ForMember((VideoOS.DataCollector.Service.Grpc.PerformanceCounterValue dest) => dest.CollectionTime, delegate(IMemberConfigurationExpression<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue, VideoOS.DataCollector.Service.Grpc.PerformanceCounterValue, Timestamp> opt)
		{
			opt.MapFrom((VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue src) => Timestamp.FromDateTime(src.CollectionTime));
		});
		CreateMap(typeof(IEnumerable<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue>), typeof(GetPerformanceCounterValuesResponse)).ConvertUsing(typeof(GetPerformanceCounterValuesResponseConverter));
		CreateMap<VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance>().ForMember((VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance dest) => dest.CategoryType, delegate(IMemberConfigurationExpression<VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance, System.Diagnostics.PerformanceCounterCategoryType> opt)
		{
			opt.MapFrom((VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance src) => (System.Diagnostics.PerformanceCounterCategoryType)(int)src.CategoryType);
		}).ForMember((VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance dest) => dest.CounterType, delegate(IMemberConfigurationExpression<VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance, System.Diagnostics.PerformanceCounterType> opt)
		{
			opt.MapFrom((VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance src) => (System.Diagnostics.PerformanceCounterType)(int)src.CounterType);
		});
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance, VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance>().ForMember((VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance dest) => dest.CategoryType, delegate(IMemberConfigurationExpression<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance, VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance, VideoOS.DataCollector.Service.Grpc.PerformanceCounterCategoryType> opt)
		{
			opt.MapFrom((VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance src) => (VideoOS.DataCollector.Service.Grpc.PerformanceCounterCategoryType)(int)src.CategoryType);
		}).ForMember((VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance dest) => dest.CounterType, delegate(IMemberConfigurationExpression<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance, VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance, VideoOS.DataCollector.Service.Grpc.PerformanceCounterType> opt)
		{
			opt.MapFrom((VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance src) => (VideoOS.DataCollector.Service.Grpc.PerformanceCounterType)(int)src.CounterType);
		});
		CreateMap<VideoOS.DataCollector.Service.Grpc.CounterStateRuleConfiguration, VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRuleConfiguration>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRuleConfiguration, VideoOS.DataCollector.Service.Grpc.CounterStateRuleConfiguration>();
		CreateMap<VideoOS.DataCollector.Service.Grpc.CounterStateRulesConfigurationUpdate, VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRulesConfigurationUpdate>();
		CreateMap<VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRulesConfigurationUpdate, VideoOS.DataCollector.Service.Grpc.CounterStateRulesConfigurationUpdate>();
	}
}
