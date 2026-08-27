using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmDataSettings", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class AlarmDataSettings : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private AlarmCategory[] CategoriesField;

	private AlarmListColumn[] ListColumnsField;

	private AlarmPriority[] PrioritiesField;

	private bool ReasonCodeEnabledField;

	private string[] ReasonCodesField;

	private AlarmState[] StatesField;

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
	public AlarmCategory[] Categories
	{
		get
		{
			return CategoriesField;
		}
		set
		{
			CategoriesField = value;
		}
	}

	[DataMember]
	public AlarmListColumn[] ListColumns
	{
		get
		{
			return ListColumnsField;
		}
		set
		{
			ListColumnsField = value;
		}
	}

	[DataMember]
	public AlarmPriority[] Priorities
	{
		get
		{
			return PrioritiesField;
		}
		set
		{
			PrioritiesField = value;
		}
	}

	[DataMember]
	public bool ReasonCodeEnabled
	{
		get
		{
			return ReasonCodeEnabledField;
		}
		set
		{
			ReasonCodeEnabledField = value;
		}
	}

	[DataMember]
	public string[] ReasonCodes
	{
		get
		{
			return ReasonCodesField;
		}
		set
		{
			ReasonCodesField = value;
		}
	}

	[DataMember]
	public AlarmState[] States
	{
		get
		{
			return StatesField;
		}
		set
		{
			StatesField = value;
		}
	}
}
