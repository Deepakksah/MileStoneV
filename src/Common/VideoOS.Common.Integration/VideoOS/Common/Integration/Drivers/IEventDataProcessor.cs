namespace VideoOS.Common.Integration.Drivers;

public interface IEventDataProcessor
{
	void ProcessEventData(string eventXmlFragment);
}
