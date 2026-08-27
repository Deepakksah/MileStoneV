using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("MatrixMonitorInfo: {DisplayName} ({MatrixMonitorId})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class MatrixMonitorInfo
{
	[DataMember]
	public Guid MatrixMonitorId { get; set; }

	[DataMember]
	public string DisplayName { get; set; }
}
