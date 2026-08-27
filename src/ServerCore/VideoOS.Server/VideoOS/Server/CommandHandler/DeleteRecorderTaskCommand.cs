using System;
using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class DeleteRecorderTaskCommand : SystemCallContextInfoTaskCommandBase
{
	public Guid RecordingServerId { get; private set; }

	public string RecordingServerName { get; private set; }

	public bool UserAcceptedToDeleteMarkedData { get; private set; }

	public DeleteRecorderTaskCommand(string taskXml)
		: base(taskXml)
	{
		base.MaxRetryCount = 1;
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "recordingServerId");
		RecordingServerName = ReadTaskStringNodeValue(taskNode, "recordingServerName");
		UserAcceptedToDeleteMarkedData = ReadTaskBoolNodeValue(taskNode, "deleteMarkedData");
	}
}
