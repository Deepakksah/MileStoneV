using System;
using System.Xml.XPath;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class ChangeHardwarePasswordTaskCommand : SystemTaskCommandBase, ICommandWithSource
{
	public Guid RecordingServerId { get; private set; }

	public Guid HardwareId { get; private set; }

	public string HardwareName { get; private set; }

	public string UserName { get; private set; }

	public string Password { get; private set; }

	public Guid SourceId => RecordingServerId;

	public ChangeHardwarePasswordTaskCommand(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "recordingServerId");
		HardwareId = ReadTaskGuidNodeValue(taskNode, "hardwareId");
		HardwareName = ReadTaskStringNodeValue(taskNode, "hardwareName");
		UserName = ReadTaskStringNodeValue(taskNode, "userName");
		Password = ReadTaskStringNodeValue(taskNode, "password");
	}
}
