using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TaskInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class TaskInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string AsyncOperationField;

	private AsyncStatusInfo AsyncStatusField;

	private DateTime EndTimeField;

	private string IdField;

	private string InformationField;

	private int PercentCompletedField;

	private DateTime StartTimeField;

	private string TargetField;

	private KeyValueInfo[] TaskPropertiesField;

	private string TypeField;

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
	public string AsyncOperation
	{
		get
		{
			return AsyncOperationField;
		}
		set
		{
			AsyncOperationField = value;
		}
	}

	[DataMember]
	public AsyncStatusInfo AsyncStatus
	{
		get
		{
			return AsyncStatusField;
		}
		set
		{
			AsyncStatusField = value;
		}
	}

	[DataMember]
	public DateTime EndTime
	{
		get
		{
			return EndTimeField;
		}
		set
		{
			EndTimeField = value;
		}
	}

	[DataMember]
	public string Id
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
	public string Information
	{
		get
		{
			return InformationField;
		}
		set
		{
			InformationField = value;
		}
	}

	[DataMember]
	public int PercentCompleted
	{
		get
		{
			return PercentCompletedField;
		}
		set
		{
			PercentCompletedField = value;
		}
	}

	[DataMember]
	public DateTime StartTime
	{
		get
		{
			return StartTimeField;
		}
		set
		{
			StartTimeField = value;
		}
	}

	[DataMember]
	public string Target
	{
		get
		{
			return TargetField;
		}
		set
		{
			TargetField = value;
		}
	}

	[DataMember]
	public KeyValueInfo[] TaskProperties
	{
		get
		{
			return TaskPropertiesField;
		}
		set
		{
			TaskPropertiesField = value;
		}
	}

	[DataMember]
	public string Type
	{
		get
		{
			return TypeField;
		}
		set
		{
			TypeField = value;
		}
	}
}
