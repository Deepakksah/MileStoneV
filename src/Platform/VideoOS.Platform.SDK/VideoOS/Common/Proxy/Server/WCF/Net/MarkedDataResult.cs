using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "MarkedDataResult", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MarkedDataResult : IMarkedDataResult
{
	private FaultDevice[] FaultDevicesField;

	private MarkedData MarkedDataField;

	private ResultStatus StatusField;

	private WarningDevice[] WarningDevicesField;

	[DataMember]
	public FaultDevice[] FaultDevices
	{
		get
		{
			return FaultDevicesField;
		}
		set
		{
			FaultDevicesField = value;
		}
	}

	[DataMember]
	public MarkedData MarkedData
	{
		get
		{
			return MarkedDataField;
		}
		set
		{
			MarkedDataField = value;
		}
	}

	[DataMember]
	public ResultStatus Status
	{
		get
		{
			return StatusField;
		}
		set
		{
			StatusField = value;
		}
	}

	[DataMember]
	public WarningDevice[] WarningDevices
	{
		get
		{
			return WarningDevicesField;
		}
		set
		{
			WarningDevicesField = value;
		}
	}

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
}
