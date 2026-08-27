using System;
using System.Threading.Tasks;
using VideoOS.DataCollector.Media;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;

namespace VideoOS.DataCollector.Service.Sample.Services.Rule;

public class NvidiaDataSampler : INvidiaSampler, IRuleSampler
{
	private INvidiaInformationWrapper _nvidiaInformationService;

	public NvidiaDataSampler(INvidiaInformationWrapper nvidiaInformationService)
	{
		_nvidiaInformationService = nvidiaInformationService;
	}

	public Task<NvidiaSample?> Sample(NvidiaSampleType type)
	{
		if (!_nvidiaInformationService.HasHACapability())
		{
			return Task.FromResult<NvidiaSample>(null);
		}
		return type switch
		{
			NvidiaSampleType.DecoderLoad => Task.FromResult(new NvidiaSample
			{
				Value = GetMaximumSample(_nvidiaInformationService.GetDecoderLoad)
			}), 
			NvidiaSampleType.DeviceLoad => Task.FromResult(new NvidiaSample
			{
				Value = GetMaximumSample(_nvidiaInformationService.GetDecoderLoad)
			}), 
			NvidiaSampleType.GetMemoryUsage => Task.FromResult(new NvidiaSample
			{
				Value = GetMaximumSample(_nvidiaInformationService.GetDecoderLoad)
			}), 
			NvidiaSampleType.None => Task.FromResult<NvidiaSample>(null), 
			_ => Task.FromResult<NvidiaSample>(null), 
		};
	}

	private uint GetMaximumSample(Func<string, uint> sampleFunction)
	{
		uint num = 0u;
		foreach (string nvidiaId in _nvidiaInformationService.NvidiaIds)
		{
			num = Math.Max(num, sampleFunction(nvidiaId));
		}
		return num;
	}
}
