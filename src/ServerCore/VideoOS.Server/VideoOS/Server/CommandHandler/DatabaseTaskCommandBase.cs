using System;
using System.IO;
using System.Xml.XPath;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public abstract class DatabaseTaskCommandBase : TaskCommandBase
{
	public TaskType TaskType { get; private set; }

	public Guid RecorderId { get; private set; }

	public Guid StorageId { get; private set; }

	protected DatabaseTaskCommandBase(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecorderId = ReadTaskGuidNodeValue(taskNode, "idRecorder");
		StorageId = ReadTaskGuidNodeValue(taskNode, "idStorage");
	}

	public static DatabaseTaskCommandBase CreateCommandFromXml(string taskXml)
	{
		if (taskXml == null)
		{
			throw new ArgumentNullException("taskXml");
		}
		DatabaseTaskCommandBase databaseTaskCommandBase;
		using (StringReader textReader = new StringReader(taskXml))
		{
			XPathDocument xPathDocument = new XPathDocument(textReader);
			XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("task");
			if (xPathNavigator2 == null)
			{
				throw new InvalidTaskXmlException("The task xml contains no 'task' node. Xml: " + taskXml);
			}
			string attribute = xPathNavigator2.GetAttribute("type", string.Empty);
			if (!(attribute == "Database.Archiving"))
			{
				if (!(attribute == "Database.Repairing2"))
				{
					throw new NotSupportedException("The task type is not supported. Type: " + attribute);
				}
				databaseTaskCommandBase = new DatabaseRepairingCommand(taskXml);
				databaseTaskCommandBase.TaskType = TaskType.Repairing;
				databaseTaskCommandBase.Initialize(xPathNavigator2);
			}
			else
			{
				databaseTaskCommandBase = new DatabaseArchivingCommand(taskXml);
				databaseTaskCommandBase.TaskType = TaskType.Archiving;
				databaseTaskCommandBase.Initialize(xPathNavigator2);
			}
		}
		return databaseTaskCommandBase;
	}
}
