using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmAction", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class AlarmAction : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid ActionTypeField;

	private string DataField;

	private Guid IdField;

	private int MapTypeField;

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
	public Guid ActionType
	{
		get
		{
			return ActionTypeField;
		}
		set
		{
			ActionTypeField = value;
		}
	}

	[DataMember]
	public string Data
	{
		get
		{
			return DataField;
		}
		set
		{
			DataField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public int MapType
	{
		get
		{
			return MapTypeField;
		}
		set
		{
			MapTypeField = value;
		}
	}
}
