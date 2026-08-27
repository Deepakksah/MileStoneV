namespace VideoOS.Management.Server;

public interface IPerformanceCounterSenderFactory
{
	IPerformanceCounterUpdateSender GetSender(int ruleType);
}
