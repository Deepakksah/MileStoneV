using System;
using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class GetDevicePresetsTaskCommand : SystemTaskCommandBase
{
	public Guid RecordingServerId { get; private set; }

	public Guid DeviceId { get; private set; }

	public long SessionDataId { get; private set; }

	public GetDevicePresetsTaskCommand(string taskXml)
		: base(taskXml)
	{
		base.MaxRetryCount = 1;
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "idRecorder");
		DeviceId = ReadTaskGuidNodeValue(taskNode, "idDevice");
		SessionDataId = ReadTaskIntegerNodeValue(taskNode, "sessionDataId");
	}
}
