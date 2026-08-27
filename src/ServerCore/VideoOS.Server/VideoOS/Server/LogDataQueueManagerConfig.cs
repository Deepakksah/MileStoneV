using System.Xml;

namespace VideoOS.Server;

public sealed class LogDataQueueManagerConfig : ILogDataQueueManagerConfig
{
	public int NumberOfLogsToRead { get; private set; }

	public LogDataQueueManagerConfig()
	{
		NumberOfLogsToRead = 20;
	}

	public void Initialize(XmlElement node)
	{
		if (node == null)
		{
			throw new ServerConfigInvalidException("LogDataQueueManager configuration missing.");
		}
		XmlElement xmlElement = node["NumberOfLogsToRead"];
		if (xmlElement != null && int.TryParse(xmlElement.InnerText, out var result))
		{
			NumberOfLogsToRead = result;
		}
	}
}
