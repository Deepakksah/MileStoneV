using System;
using System.Xml.XPath;

namespace VideoOS.Common.Integration.Command;

public class EdgeStorageRetrievalCommand : EdgeTaskCommandBase
{
	public DateTime EdgeStartTime { get; private set; }

	public DateTime EdgeEndTime { get; private set; }

	public EdgeStorageRetrievalCommand(string taskXml, bool stopping)
		: base(taskXml, stopping)
	{
		base.MaxRetryCount = 5;
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		EdgeStartTime = ReadTaskDateTimeNodeValue(taskNode, "startTime");
		EdgeEndTime = ReadTaskDateTimeNodeValue(taskNode, "endTime");
	}
}
