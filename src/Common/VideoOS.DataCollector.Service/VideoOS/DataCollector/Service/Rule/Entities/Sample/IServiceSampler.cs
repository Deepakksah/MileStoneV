namespace VideoOS.DataCollector.Service.Rule.Entities.Sample;

public interface IServiceSampler : IRuleSampler
{
	bool Sample(string serviceName);
}
