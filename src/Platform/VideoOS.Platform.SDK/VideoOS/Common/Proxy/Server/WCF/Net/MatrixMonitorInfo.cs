using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "MatrixMonitorInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MatrixMonitorInfo : IMatrixMonitorInfo
{
	private string DisplayNameField;

	private Guid MatrixMonitorIdField;

	[DataMember]
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			DisplayNameField = value;
		}
	}

	[DataMember]
	public Guid MatrixMonitorId
	{
		get
		{
			return MatrixMonitorIdField;
		}
		set
		{
			MatrixMonitorIdField = value;
		}
	}
}
