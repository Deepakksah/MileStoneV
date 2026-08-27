using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MarkedDataResult : IMarkedDataResult
{
	private MarkedData markedDataField;

	private FaultDevice[] faultDevicesField;

	private ResultStatus statusField;

	IFaultDevice[] IMarkedDataResult.FaultDevices
	{
		get
		{
			return FaultDevices;
		}
		set
		{
			FaultDevices = (FaultDevice[])value;
		}
	}

	IMarkedData IMarkedDataResult.MarkedData
	{
		get
		{
			return MarkedData;
		}
		set
		{
			MarkedData = (MarkedData)value;
		}
	}

	IResultStatus IMarkedDataResult.Status
	{
		get
		{
			return Status switch
			{
				ResultStatus.Undefined => IResultStatus.Undefined, 
				ResultStatus.Success => IResultStatus.Success, 
				ResultStatus.PartlySuccess => IResultStatus.PartlySuccess, 
				ResultStatus.Failed => IResultStatus.Failed, 
				ResultStatus.MarkedDataNotFound => IResultStatus.MarkedDataNotFound, 
				ResultStatus.FeatureNotLicensed => IResultStatus.FeatureNotLicensed, 
				ResultStatus.RecorderNotFound => IResultStatus.RecorderNotFound, 
				ResultStatus.ManagementServerUnavailable => IResultStatus.ManagementServerUnavailable, 
				ResultStatus.Unauthorized => IResultStatus.Unauthorized, 
				_ => IResultStatus.Undefined, 
			};
		}
		set
		{
			switch (value)
			{
			case IResultStatus.Undefined:
				Status = ResultStatus.Undefined;
				break;
			case IResultStatus.Success:
				Status = ResultStatus.Success;
				break;
			case IResultStatus.PartlySuccess:
				Status = ResultStatus.PartlySuccess;
				break;
			case IResultStatus.Failed:
				Status = ResultStatus.Failed;
				break;
			case IResultStatus.MarkedDataNotFound:
				Status = ResultStatus.MarkedDataNotFound;
				break;
			case IResultStatus.FeatureNotLicensed:
				Status = ResultStatus.FeatureNotLicensed;
				break;
			case IResultStatus.RecorderNotFound:
				Status = ResultStatus.RecorderNotFound;
				break;
			case IResultStatus.ManagementServerUnavailable:
				Status = ResultStatus.ManagementServerUnavailable;
				break;
			case IResultStatus.Unauthorized:
				Status = ResultStatus.Unauthorized;
				break;
			default:
				Status = ResultStatus.Undefined;
				break;
			}
		}
	}

	public MarkedData MarkedData
	{
		get
		{
			return markedDataField;
		}
		set
		{
			markedDataField = value;
		}
	}

	public FaultDevice[] FaultDevices
	{
		get
		{
			return faultDevicesField;
		}
		set
		{
			faultDevicesField = value;
		}
	}

	public ResultStatus Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}
}
