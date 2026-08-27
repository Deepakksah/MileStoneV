using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MatrixMonitorInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MatrixMonitorInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DisplayNameField;

	private Guid MatrixMonitorIdField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

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
