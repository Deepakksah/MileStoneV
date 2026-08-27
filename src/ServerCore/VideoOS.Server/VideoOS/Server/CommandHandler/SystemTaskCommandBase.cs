using System;
using System.IO;
using System.Xml.XPath;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public abstract class SystemTaskCommandBase : TaskCommandBase
{
	public TaskType TaskType { get; private set; }

	protected SystemTaskCommandBase(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
	}

	public static SystemTaskCommandBase CreateCommandFromXml(string taskXml)
	{
		if (taskXml == null)
		{
			throw new ArgumentNullException("taskXml");
		}
		SystemTaskCommandBase systemTaskCommandBase;
		using (StringReader textReader = new StringReader(taskXml))
		{
			XPathDocument xPathDocument = new XPathDocument(textReader);
			XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("task");
			if (xPathNavigator2 == null)
			{
				throw new InvalidTaskXmlException("The task xml contains no 'task' node.");
			}
			string attribute = xPathNavigator2.GetAttribute("type", string.Empty);
			switch (attribute)
			{
			case "System.AddHardware":
				systemTaskCommandBase = new AddHardwareTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.AddHardware;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.MoveHardware":
				systemTaskCommandBase = new MoveHardwareTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.MoveHardware;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.ChangeHardwarePassword":
				systemTaskCommandBase = new ChangeHardwarePasswordTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.ChangeHardwarePassword;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.ActivateLicense":
				systemTaskCommandBase = new ActivateLicenseTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.ActivateLicense;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.HardwareScan":
				systemTaskCommandBase = new HardwareScanTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.HardwareScan;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.HardwareScanExpress":
				systemTaskCommandBase = new HardwareScanExpressTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.HardwareScanExpress;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.DeleteHardware":
				systemTaskCommandBase = new DeleteHardwareTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.DeleteHardware;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.UpdateHardware":
				systemTaskCommandBase = new UpdateHardwareTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.UpdateHardware;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.ReplaceHardware":
				systemTaskCommandBase = new ReplaceHardwareTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.ReplaceHardware;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.FirmwareUpgrade":
				systemTaskCommandBase = new FirmwareUpgradeTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.FirmwareUpgrade;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.AddIdpAdmin":
				systemTaskCommandBase = new ChangeIdpAdminTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.AddIdpAdmin;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.DeleteIdpAdmin":
				systemTaskCommandBase = new ChangeIdpAdminTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.DeleteIdpAdmin;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.DeleteIdpUser":
				systemTaskCommandBase = new DeleteIdpUserTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.DeleteIdpUser;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.GetDevicePresets":
				systemTaskCommandBase = new GetDevicePresetsTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.GetDevicePresets;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			case "System.DeleteRecorder":
				systemTaskCommandBase = new DeleteRecorderTaskCommand(taskXml);
				systemTaskCommandBase.TaskType = TaskType.DeleteRecorder;
				systemTaskCommandBase.Permanent = true;
				systemTaskCommandBase.Initialize(xPathNavigator2);
				break;
			default:
				throw new NotSupportedException("The task type is not supported. Type: " + attribute);
			}
		}
		return systemTaskCommandBase;
	}
}
