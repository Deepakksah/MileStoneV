using System;
using System.Xml.XPath;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.DeviceDiscovery;

namespace VideoOS.Server.CommandHandler;

public sealed class HardwareScanExpressTaskCommand : SystemTaskCommandBase, IStoppableTaskCommand, ITaskCommandBase
{
	public Guid RecordingServerId { get; private set; }

	public Credentials Credentials { get; private set; }

	public bool UseDefaultCredentials { get; private set; }

	public bool UseSecurePort { get; private set; }

	public ScanPriorities PrioritizedDrivers { get; private set; }

	public bool Stopping { get; private set; }

	public event EventHandler<StoppableTaskCommandEventArgs> TaskStopped;

	public HardwareScanExpressTaskCommand(string taskXml)
		: base(taskXml)
	{
		Stopping = false;
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "idRecorder");
		string xml = ReadTaskStringNodeValue(taskNode, "credentialsListXml");
		Credentials = Credentials.FromXml(xml);
		UseDefaultCredentials = ReadTaskBoolNodeValue(taskNode, "useDefaultCredentials");
		UseSecurePort = ReadTaskBoolNodeValue(taskNode, "useSecurePort");
		PrioritizedDrivers = ScanPriorities.FromXml(ReadTaskStringNodeValue(taskNode, "prioritizedDriversXml"));
	}

	public void StopTask()
	{
		if (!Stopping)
		{
			Stopping = true;
			this.TaskStopped?.Invoke(this, new StoppableTaskCommandEventArgs(base.TaskId));
		}
	}
}
