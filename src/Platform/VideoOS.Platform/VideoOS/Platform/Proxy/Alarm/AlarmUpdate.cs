using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmUpdate", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class AlarmUpdate : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string KeyField;

	private string ValueField;

	private DateTime TimeField;

	private string AuthorField;

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
	public string Key
	{
		get
		{
			return KeyField;
		}
		set
		{
			KeyField = value;
		}
	}

	[DataMember]
	public string Value
	{
		get
		{
			return ValueField;
		}
		set
		{
			ValueField = value;
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
}
