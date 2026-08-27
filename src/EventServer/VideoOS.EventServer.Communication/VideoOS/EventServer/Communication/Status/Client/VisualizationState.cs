using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "VisualizationState", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class VisualizationState : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private DateTime TimestampField;

	private string VisualizationField;

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
	public DateTime Timestamp
	{
		get
		{
			return TimestampField;
		}
		set
		{
			TimestampField = value;
		}
	}

	[DataMember]
	public string Visualization
	{
		get
		{
			return VisualizationField;
		}
		set
		{
			VisualizationField = value;
		}
	}
}
