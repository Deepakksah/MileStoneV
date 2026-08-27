using System;
using System.ServiceModel;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.WCF.Server;

[ServiceContract(Name = "IServerCommand", Namespace = "http://videoos.net/2/LPRCommand", SessionMode = SessionMode.NotAllowed)]
public interface IServerCommand
{
	DateTime LastDetectedLicensePlatesPollTime { get; set; }

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetVersionResp GetVersion(string token, GetVersionParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetStatusResp GetStatus(string token, GetStatusParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetIntradaCountryModulesResp GetIntradaCountryModules(string token, GetIntradaCountryModulesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	GetDetectedLicensePlatesResp GetDetectedLicensePlates(string token, GetDetectedLicensePlatesParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	SystemConfigurationHasChangedResp SystemConfigurationHasChanged(string token, SystemConfigurationHasChangedParams parameters);

	[OperationContract]
	[FaultContract(typeof(ServiceFault))]
	AnalyzeJpegResp AnalyzeJpeg(string token, AnalyzeJpegParams parameters);
}
