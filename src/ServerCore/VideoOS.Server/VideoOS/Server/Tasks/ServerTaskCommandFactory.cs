using System;
using System.IO;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Tasks;
using VideoOS.Server.CommandHandler;

namespace VideoOS.Server.Tasks;

public class ServerTaskCommandFactory : ITaskCommandFactory
{
	private ILogger logger;

	public ServerTaskCommandFactory(ILogger logger)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		this.logger = logger;
	}

	public TaskCommandBase CreateTaskCommand(ServerTaskDto task)
	{
		if (task == null)
		{
			throw new ArgumentNullException("task");
		}
		using StringReader textReader = new StringReader(task.TaskXml);
		XPathDocument xPathDocument = new XPathDocument(textReader);
		XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
		xPathNavigator = xPathNavigator.SelectSingleNode("task");
		string attribute = xPathNavigator.GetAttribute("type", string.Empty);
		if (attribute.StartsWith("System.", StringComparison.Ordinal))
		{
			return SystemTaskCommandBase.CreateCommandFromXml(task.TaskXml);
		}
		logger.LogError("The task type specified in the task is not supported. Type: {TaskType}", attribute);
		throw new NotSupportedException("The task type specified in the task is not supported");
	}
}
