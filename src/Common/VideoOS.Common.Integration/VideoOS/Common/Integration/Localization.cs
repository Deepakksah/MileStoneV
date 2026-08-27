using System;
using System.Globalization;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Properties;
using VideoOS.Common.Services;

namespace VideoOS.Common.Integration;

public static class Localization
{
	public enum StringNodeType
	{
		RootNode,
		AllNode,
		NewGroupName,
		DefaultName
	}

	public enum StringNodeTypeClientProfile
	{
		RootNode,
		DefaultClientProfile
	}

	public static string MapDeviceTypeToString(string deviceType, StringNodeType type)
	{
		string text = "";
		return RegisteredDeviceType.GetDeviceTypeEnum(deviceType) switch
		{
			DeviceTypeEnum.Camera => type switch
			{
				StringNodeType.RootNode => Resources.TopNodeInTreeCameraes, 
				StringNodeType.AllNode => Resources.AllNodeInTreeCameras, 
				StringNodeType.NewGroupName => Resources.NewGroupNameCameras, 
				_ => Resources.CameraDefaultName, 
			}, 
			DeviceTypeEnum.Input => type switch
			{
				StringNodeType.RootNode => Resources.TopNodeInTreeInputs, 
				StringNodeType.AllNode => Resources.AllNodeInTreeInputs, 
				StringNodeType.NewGroupName => Resources.NewGroupNameInputs, 
				_ => Resources.InputDefaultName, 
			}, 
			DeviceTypeEnum.Output => type switch
			{
				StringNodeType.RootNode => Resources.TopNodeInTreeOutputs, 
				StringNodeType.AllNode => Resources.AllNodeInTreeOutputs, 
				StringNodeType.NewGroupName => Resources.NewGroupNameOutputs, 
				_ => Resources.OutputDefaultName, 
			}, 
			DeviceTypeEnum.AudioInput => type switch
			{
				StringNodeType.RootNode => Resources.TopNodeInTreeMicrophones, 
				StringNodeType.AllNode => Resources.AllNodeInTreeMicrophones, 
				StringNodeType.NewGroupName => Resources.NewGroupNameMicrophones, 
				_ => Resources.MicrophoneDefaultName, 
			}, 
			DeviceTypeEnum.AudioOutput => type switch
			{
				StringNodeType.RootNode => Resources.TopNodeInTreeSpeakers, 
				StringNodeType.AllNode => Resources.AllNodeInTreeSpeakers, 
				StringNodeType.NewGroupName => Resources.NewGroupNameSpeakers, 
				_ => Resources.SpeakerDefaultName, 
			}, 
			DeviceTypeEnum.Metadata => type switch
			{
				StringNodeType.RootNode => Resources.TopNodeInTreeMetadata, 
				StringNodeType.AllNode => Resources.AllNodeInTreeMetadata, 
				StringNodeType.NewGroupName => Resources.NewGroupNameMetadata, 
				_ => Resources.MetadataDefaultName, 
			}, 
			_ => type switch
			{
				StringNodeType.RootNode => Resources.TopNodeInTreeDefault, 
				StringNodeType.AllNode => Resources.AllNodeInTreeDefault, 
				StringNodeType.NewGroupName => Resources.NewGroupNameDefault, 
				_ => Resources.DefaultDefaultName, 
			}, 
		};
	}

	public static string MapFailoverToString(StringNodeType type)
	{
		string text = "";
		return type switch
		{
			StringNodeType.RootNode => Resources.TopNodeInTreeFailover, 
			StringNodeType.NewGroupName => Resources.NewGroupNameFailover, 
			_ => "", 
		};
	}

	public static string MapServiceTypeToString(Guid type)
	{
		if (type.Equals(ServiceTypes.ServiceType_EventServerService))
		{
			return Resources.ServiceTypeEventServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_LegacyLogService) || type.Equals(ServiceTypes.ServiceType_LogService))
		{
			return Resources.ServiceTypeLogServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_TransactService))
		{
			return Resources.ServiceTypeTransactServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_RegisteredService))
		{
			return Resources.ServiceTypeRegisteredServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_ReportService))
		{
			return Resources.ServiceTypeReportServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_IncidentManagerService))
		{
			return Resources.ServiceTypeIncidentManagerServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_MobileService))
		{
			return Resources.ServiceTypeMobileServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_OnvifService))
		{
			return Resources.ServiceTypeOnvifServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_LicensePlateRecognitionService))
		{
			return Resources.ServiceTypeLicensePlateRecognitionServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_FailoverService))
		{
			return Resources.ServiceTypeFailoverServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_GatewayService))
		{
			return Resources.ServiceTypeGatewayServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_ProcessingServerService))
		{
			return Resources.ServiceTypeProcessingServerServiceName;
		}
		if (type.Equals(ServiceTypes.ServiceType_AlarmsService))
		{
			return Resources.ServiceTypeAlarmsServiceName;
		}
		return Resources.ServiceTypeUnknownServiceName;
	}

	public static string MapServiceTypeToNameWithType(string name, Guid serverType)
	{
		if (serverType == ServiceTypes.ServiceType_LicensePlateRecognitionService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeLicensePlateRecognitionServiceName);
		}
		if (serverType == ServiceTypes.ServiceType_EventServerService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeEventServiceName);
		}
		if (serverType == ServiceTypes.ServiceType_LegacyLogService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeLogServiceName);
		}
		if (serverType == ServiceTypes.ServiceType_MobileService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeMobileServiceName);
		}
		if (serverType == ServiceTypes.ServiceType_ReportService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeReportServiceName);
		}
		if (serverType == ServiceTypes.ServiceType_IncidentManagerService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeIncidentManagerServiceName);
		}
		if (serverType == ServiceTypes.ServiceType_LogService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeLogServiceName);
		}
		if (serverType == ServiceTypes.ServiceType_GatewayService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeGatewayServiceName);
		}
		if (serverType == ServiceTypes.ServiceType_ProcessingServerService)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.ServiceName_Format, name, Resources.ServiceTypeProcessingServerServiceName);
		}
		return Resources.ServiceTypeUnknownServiceName;
	}

	public static string MapServiceTypeToGroupName(Guid serverType)
	{
		if (serverType == ServiceTypes.ServiceType_LicensePlateRecognitionService)
		{
			return Resources.ServiceTypeLPRServiceGroupName;
		}
		if (serverType == ServiceTypes.ServiceType_EventServerService)
		{
			return Resources.ServiceTypeEventServiceGroupName;
		}
		if (serverType == ServiceTypes.ServiceType_LegacyLogService)
		{
			return Resources.ServiceTypeLogServiceGroupName;
		}
		if (serverType == ServiceTypes.ServiceType_MobileService)
		{
			return Resources.ServiceTypeMobileServiceGroupName;
		}
		if (serverType == ServiceTypes.ServiceType_ReportService)
		{
			return Resources.ServiceTypeReportingServiceGroupName;
		}
		if (serverType == ServiceTypes.ServiceType_IncidentManagerService)
		{
			return Resources.ServiceTypeIncidentManagerServiceGroupName;
		}
		if (serverType == ServiceTypes.ServiceType_ManagementService)
		{
			return Resources.AllManagementServers;
		}
		if (serverType == ServiceTypes.ServiceType_RecordingService)
		{
			return Resources.AllRecordingServers;
		}
		if (serverType == ServiceTypes.ServiceType_LogService)
		{
			return Resources.ServiceTypeLogServiceGroupName;
		}
		if (serverType == ServiceTypes.ServiceType_GatewayService)
		{
			return Resources.AllGatewayServices;
		}
		if (serverType == ServiceTypes.ServiceType_ProcessingServerService)
		{
			return Resources.ServiceTypeProcessingServerServiceGroupName;
		}
		return Resources.ServiceTypeUnknownServiceName;
	}
}
