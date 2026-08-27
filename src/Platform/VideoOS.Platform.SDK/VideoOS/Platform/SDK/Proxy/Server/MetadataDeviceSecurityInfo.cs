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
public class MetadataDeviceSecurityInfo : IMetadataDeviceSecurityInfo
{
	private bool retentionViewField;

	private bool retentionCreateField;

	private bool retentionRemoveField;

	public bool Browse { get; set; }

	public bool ExportDatabase { get; set; }

	public bool GetSequences { get; set; }

	public bool Live { get; set; }

	public bool RetrieveEdgeRecordings { get; set; }

	public bool StartRecording { get; set; }

	public bool StopRecording { get; set; }

	public bool RetentionView
	{
		get
		{
			return retentionViewField;
		}
		set
		{
			retentionViewField = value;
		}
	}

	public bool RetentionCreate
	{
		get
		{
			return retentionCreateField;
		}
		set
		{
			retentionCreateField = value;
		}
	}

	public bool RetentionRemove
	{
		get
		{
			return retentionRemoveField;
		}
		set
		{
			retentionRemoveField = value;
		}
	}
}
