using System;
using System.Xml.XPath;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.DeviceDiscovery;

namespace VideoOS.Server.CommandHandler;

public sealed class HardwareScanTaskCommand : SystemTaskCommandBase, IStoppableTaskCommand, ITaskCommandBase
{
	public Guid RecordingServerId { get; private set; }

	public Uri Address { get; private set; }

	public string UserName { get; private set; }

	public string Password { get; private set; }

	public bool UseDefaultCredentials { get; private set; }

	public ScanPriorities SelectedDrivers { get; private set; }

	public ScanPriorities PrioritizedDrivers { get; private set; }

	public AddressDriverPairs AddressDriverPairs { get; private set; }

	public bool Stopping { get; private set; }

	public event EventHandler<StoppableTaskCommandEventArgs> TaskStopped;

	public HardwareScanTaskCommand(string taskXml)
		: base(taskXml)
	{
		Stopping = false;
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "idRecorder");
		Address = ReadTaskUriNodeValue(taskNode, "address");
		UserName = ReadTaskStringNodeValue(taskNode, "userName");
		Password = ReadTaskStringNodeValue(taskNode, "password");
		UseDefaultCredentials = ReadTaskBoolNodeValue(taskNode, "useDefaultCredentials");
		SelectedDrivers = ScanPriorities.FromXml(ReadTaskStringNodeValue(taskNode, "selectedDriversXml"));
		PrioritizedDrivers = ScanPriorities.FromXml(ReadTaskStringNodeValue(taskNode, "prioritizedDriversXml"));
		if (TryReadTaskStringNodeValue(taskNode, "addressWithDriverListXml", out var nodeValue))
		{
			AddressDriverPairs = AddressDriverPairs.FromXml(nodeValue);
		}
		else
		{
			AddressDriverPairs = new AddressDriverPairs();
		}
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
