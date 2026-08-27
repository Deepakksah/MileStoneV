using System;
using System.Xml.XPath;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class MoveHardwareTaskCommand : SystemTaskCommandBase, ICommandWithSource
{
	public Guid HardwareId { get; private set; }

	public string HardwareName { get; private set; }

	public Guid SourceRecorderId { get; private set; }

	public string SourceRecorderName { get; private set; }

	public Guid DestinationRecorderId { get; private set; }

	public string DestinationRecorderName { get; private set; }

	public Guid DestinationStorageId { get; private set; }

	public string DestinationStorageName { get; private set; }

	public bool IgnoreSourceRecordingServer { get; private set; }

	Guid ICommandWithSource.SourceId => HardwareId;

	public MoveHardwareTaskCommand(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		HardwareId = ReadTaskGuidNodeValue(taskNode, "hardwareId");
		HardwareName = ReadTaskStringNodeValue(taskNode, "hardwareName");
		SourceRecorderId = ReadTaskGuidNodeValue(taskNode, "sourceRecorderId");
		SourceRecorderName = ReadTaskStringNodeValue(taskNode, "sourceRecorderName");
		DestinationRecorderId = ReadTaskGuidNodeValue(taskNode, "destinationRecorderId");
		DestinationRecorderName = ReadTaskStringNodeValue(taskNode, "destinationRecorderName");
		DestinationStorageId = ReadTaskGuidNodeValue(taskNode, "destinationStorageId");
		DestinationStorageName = ReadTaskStringNodeValue(taskNode, "destinationStorageName");
		IgnoreSourceRecordingServer = ReadTaskBoolNodeValue(taskNode, "ignoreSourceRecordingServer");
	}
}
