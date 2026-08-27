namespace VideoOS.DataCollector.Service.Common.BackOff;

public interface IBackOffFactory
{
	IBackOffProvider Create(int delayMilliseconds, int maxDelayMilliseconds);
}
