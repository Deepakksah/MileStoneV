using System;
using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class AddHardwareTaskCommand : SystemCallContextInfoTaskCommandBase
{
	public Guid RecordingServerId { get; private set; }

	public Uri Address { get; private set; }

	public string UserName { get; private set; }

	public string Password { get; private set; }

	public int DriverNumber { get; private set; }

	public string DefinitionXml { get; private set; }

	public string CustomData { get; private set; }

	public AddHardwareTaskCommand(string taskXml)
		: base(taskXml)
	{
		base.MaxRetryCount = 1;
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "idRecorder");
		Address = ReadTaskUriNodeValue(taskNode, "address");
		UserName = ReadTaskStringNodeValue(taskNode, "userName");
		Password = ReadTaskStringNodeValue(taskNode, "password");
		DriverNumber = ReadTaskIntegerNodeValue(taskNode, "driverNumber");
		DefinitionXml = ReadTaskStringNodeValue(taskNode, "definitionXml");
		CustomData = ReadTaskStringNodeValue(taskNode, "customdata");
	}
}
