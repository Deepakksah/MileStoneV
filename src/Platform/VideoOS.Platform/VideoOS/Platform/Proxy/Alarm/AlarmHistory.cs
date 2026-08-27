using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmHistory", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class AlarmHistory : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string AssignedToField;

	private string AuthorField;

	private int PriorityField;

	private int StateField;

	private string TextField;

	private DateTime TimeField;

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
	public string AssignedTo
	{
		get
		{
			return AssignedToField;
		}
		set
		{
			AssignedToField = value;
		}
	}

	[DataMember]
	public string Author
	{
		get
		{
			return AuthorField;
		}
		set
		{
			AuthorField = value;
		}
	}

	[DataMember]
	public int Priority
	{
		get
		{
			return PriorityField;
		}
		set
		{
			PriorityField = value;
		}
	}

	[DataMember]
	public int State
	{
		get
		{
			return StateField;
		}
		set
		{
			StateField = value;
		}
	}

	[DataMember]
	public string Text
	{
		get
		{
			return TextField;
		}
		set
		{
			TextField = value;
		}
	}

	[DataMember]
	public DateTime Time
	{
		get
		{
			return TimeField;
		}
		set
		{
			TimeField = value;
		}
	}
}
