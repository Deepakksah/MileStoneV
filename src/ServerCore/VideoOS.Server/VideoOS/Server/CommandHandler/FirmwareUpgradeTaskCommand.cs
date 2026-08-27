using System;
using System.Xml.XPath;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class FirmwareUpgradeTaskCommand : SystemCallContextInfoTaskCommandBase, ICommandWithSource
{
	public Guid RecordingServerId { get; private set; }

	public Guid HardwareId { get; private set; }

	public string HardwareName { get; private set; }

	public string FileId { get; private set; }

	public Guid SourceId => HardwareId;

	public FirmwareUpgradeTaskCommand(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "recordingServerId");
		HardwareId = ReadTaskGuidNodeValue(taskNode, "hardwareId");
		HardwareName = ReadTaskStringNodeValue(taskNode, "hardwareName");
		FileId = ReadTaskStringNodeValue(taskNode, "fileId");
	}
}
