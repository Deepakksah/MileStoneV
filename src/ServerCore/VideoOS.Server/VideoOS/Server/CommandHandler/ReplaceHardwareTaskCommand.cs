using System;
using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class ReplaceHardwareTaskCommand : SystemTaskCommandBase
{
	public Guid HardwareId { get; private set; }

	public Guid RecordingServerId { get; private set; }

	public string RecordingServerName { get; private set; }

	public string HardwareName { get; private set; }

	public int Port { get; private set; }

	public int HttpsPort { get; private set; }

	public string HardwareAddress { get; private set; }

	public string UserName { get; private set; }

	public string Password { get; private set; }

	public bool UseHttps { get; private set; }

	public int DriverNumber { get; private set; }

	public string CustomDriverData { get; private set; }

	public bool AllowDeletingDisabledDevices { get; private set; }

	public string ActingUserSid { get; private set; }

	public string ActingUserDomainAccountName { get; private set; }

	public string ActingUserLocationIPAddress { get; private set; }

	public ReplaceHardwareTaskCommand(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		HardwareId = ReadTaskGuidNodeValue(taskNode, "hardwareId");
		HardwareName = ReadTaskStringNodeValue(taskNode, "hardwareName");
		RecordingServerId = ReadTaskGuidNodeValue(taskNode, "recordingServerId");
		RecordingServerName = ReadTaskStringNodeValue(taskNode, "recordingServerName");
		HardwareAddress = ReadTaskStringNodeValue(taskNode, "hardwareAddress");
		Port = ReadTaskIntegerNodeValue(taskNode, "port");
		UseHttps = ReadTaskBoolNodeValue(taskNode, "useHttps");
		HttpsPort = ReadTaskIntegerNodeValue(taskNode, "httpsPort");
		UserName = ReadTaskStringNodeValue(taskNode, "userName");
		Password = ReadTaskStringNodeValue(taskNode, "password");
		DriverNumber = ReadTaskIntegerNodeValue(taskNode, "driverNumber");
		CustomDriverData = ReadTaskStringNodeValue(taskNode, "customDriverData");
		AllowDeletingDisabledDevices = ReadTaskBoolNodeValue(taskNode, "allowDeletingDisabledDevices");
		ActingUserSid = ReadTaskStringNodeValue(taskNode, "actingUserSid");
		ActingUserDomainAccountName = ReadTaskStringNodeValue(taskNode, "actingUserDomainAccountName");
		ActingUserLocationIPAddress = ReadTaskStringNodeValue(taskNode, "actingUserLocationIPAddress");
	}
}
