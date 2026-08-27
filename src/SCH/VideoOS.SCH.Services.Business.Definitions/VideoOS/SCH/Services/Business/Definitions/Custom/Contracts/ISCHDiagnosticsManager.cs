using System.Net.Security;
using System.ServiceModel;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[ServiceContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics", SessionMode = SessionMode.NotAllowed)]
public interface ISCHDiagnosticsManager
{
	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetServerInfoMsgOut GetServerInfo();

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetIdentitiesMsgOut GetIdentities();

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetServerAppDataMsgOut GetServerAppData();

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetSessionMsgIndicatorDataMsgOut GetSessionMsgIndicatorData();

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetProcessDetailsMsgOut GetProcessDetails();

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetConfigSettingsMsgOut GetConfigSettings();

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	TestTextLoggerMsgOut TestTextLogger();

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	TestWindowsEventLoggerMsgOut TestWindowsEventLogger();

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	PingMsgOut Ping(string pingText);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetAssemblyInfoMsgOut GetAssemblyInfo();
}
