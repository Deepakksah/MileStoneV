using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.WCF.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/LPRCommand", ConfigurationName = "VideoOS.LPR.Common.WCF.Client.IServerCommand", SessionMode = SessionMode.NotAllowed)]
public interface IServerCommand
{
	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetVersion", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/GetVersionResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetVersionServiceFaultFault", Name = "ServiceFault")]
	GetVersionResp GetVersion(string token, GetVersionParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetVersion", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/GetVersionResponse")]
	Task<GetVersionResp> GetVersionAsync(string token, GetVersionParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetStatus", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/GetStatusResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetStatusServiceFaultFault", Name = "ServiceFault")]
	GetStatusResp GetStatus(string token, GetStatusParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetStatus", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/GetStatusResponse")]
	Task<GetStatusResp> GetStatusAsync(string token, GetStatusParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetIntradaCountryModules", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/GetIntradaCountryModulesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetIntradaCountryModulesServiceFaultFault", Name = "ServiceFault")]
	GetIntradaCountryModulesResp GetIntradaCountryModules(string token, GetIntradaCountryModulesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetIntradaCountryModules", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/GetIntradaCountryModulesResponse")]
	Task<GetIntradaCountryModulesResp> GetIntradaCountryModulesAsync(string token, GetIntradaCountryModulesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetDetectedLicensePlates", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/GetDetectedLicensePlatesResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetDetectedLicensePlatesServiceFaultFault", Name = "ServiceFault")]
	GetDetectedLicensePlatesResp GetDetectedLicensePlates(string token, GetDetectedLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/GetDetectedLicensePlates", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/GetDetectedLicensePlatesResponse")]
	Task<GetDetectedLicensePlatesResp> GetDetectedLicensePlatesAsync(string token, GetDetectedLicensePlatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/SystemConfigurationHasChanged", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/SystemConfigurationHasChangedResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IServerCommand/SystemConfigurationHasChangedServiceFaultFault", Name = "ServiceFault")]
	SystemConfigurationHasChangedResp SystemConfigurationHasChanged(string token, SystemConfigurationHasChangedParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/SystemConfigurationHasChanged", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/SystemConfigurationHasChangedResponse")]
	Task<SystemConfigurationHasChangedResp> SystemConfigurationHasChangedAsync(string token, SystemConfigurationHasChangedParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/AnalyzeJpeg", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/AnalyzeJpegResponse")]
	[FaultContract(typeof(ServiceFault), Action = "http://videoos.net/2/LPRCommand/IServerCommand/AnalyzeJpegServiceFaultFault", Name = "ServiceFault")]
	AnalyzeJpegResp AnalyzeJpeg(string token, AnalyzeJpegParams parameters);

	[OperationContract(Action = "http://videoos.net/2/LPRCommand/IServerCommand/AnalyzeJpeg", ReplyAction = "http://videoos.net/2/LPRCommand/IServerCommand/AnalyzeJpegResponse")]
	Task<AnalyzeJpegResp> AnalyzeJpegAsync(string token, AnalyzeJpegParams parameters);
}
