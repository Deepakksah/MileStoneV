using System;
using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class UpdateHardwareTaskCommand : SystemTaskCommandBase
{
	public Guid HardwareId { get; private set; }

	public string HardwareName { get; private set; }

	public Guid RecordingServerId { get; private set; }

	public string RecordingServerName { get; private set; }

	public int SessionDataId { get; private set; }

	public UpdateHardwareTaskCommand(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		HardwareId = ReadTaskGuidNodeValue(taskNode, "hardwareId");
		HardwareName = ReadTaskStringNodeValue(taskNode, "hardwareName");
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "recordingServerId");
		RecordingServerName = ReadTaskStringNodeValue(taskNode, "recordingServerName");
		SessionDataId = ReadTaskIntegerNodeValue(taskNode, "sessionDataId");
	}
}
