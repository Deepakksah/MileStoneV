using System;
using System.IO;
using System.Xml.XPath;

namespace VideoOS.Common.Integration.Command;

public abstract class EdgeTaskCommandBase : TaskCommandBase, IStoppableTaskCommand, ITaskCommandBase
{
	public EdgeTaskType TaskType { get; private set; }

	public Guid RecorderId { get; private set; }

	public Guid DeviceId { get; private set; }

	public bool Stopping { get; private set; }

	public event EventHandler<StoppableTaskCommandEventArgs> TaskStopped;

	protected EdgeTaskCommandBase(string taskXml, bool stopping)
		: base(taskXml)
	{
		Stopping = stopping;
	}

	public void StopTask()
	{
		if (!Stopping)
		{
			Stopping = true;
			this.TaskStopped?.Invoke(this, new StoppableTaskCommandEventArgs(base.TaskId));
		}
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecorderId = ReadTaskGuidNodeValue(taskNode, "target");
		DeviceId = ReadTaskGuidNodeValue(taskNode, "idDevice");
	}

	public static EdgeTaskCommandBase CreateCommandFromXml(string taskXml, bool stopping)
	{
		if (taskXml == null)
		{
			throw new ArgumentNullException("taskXml");
		}
		using StringReader textReader = new StringReader(taskXml);
		XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator().SelectSingleNode("task");
		if (xPathNavigator == null)
		{
			throw new InvalidTaskXmlException("The task xml contains no 'task' node. Xml: " + taskXml);
		}
		string attribute = xPathNavigator.GetAttribute("type", string.Empty);
		if (attribute == "Edge.StorageRetrieval")
		{
			EdgeTaskCommandBase edgeTaskCommandBase = new EdgeStorageRetrievalCommand(taskXml, stopping);
			edgeTaskCommandBase.TaskType = EdgeTaskType.StorageRetrieval;
			edgeTaskCommandBase.Permanent = true;
			edgeTaskCommandBase.Initialize(xPathNavigator);
			return edgeTaskCommandBase;
		}
		throw new NotSupportedException("The task type is not supported. Type: " + attribute);
	}
}
