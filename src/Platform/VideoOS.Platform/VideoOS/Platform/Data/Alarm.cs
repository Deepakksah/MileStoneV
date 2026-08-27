using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Alarm", Namespace = "urn:milestone-systems")]
public class Alarm : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private EventHeader EventHeaderField;

	private string DescriptionField;

	private DateTime StartTimeField;

	private DateTime EndTimeField;

	private string LocationField;

	private ushort StateField;

	private string StateNameField;

	private ushort CategoryField;

	private string CategoryNameField;

	private string AssignedToField;

	private uint CountField;

	private RuleList RuleListField;

	private AnalyticsObjectList ObjectListField;

	private ReferenceList ReferenceListField;

	private SnapshotList SnapshotListField;

	private Vendor VendorField;

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

	[DataMember(IsRequired = true, EmitDefaultValue = false)]
	public EventHeader EventHeader
	{
		get
		{
			return EventHeaderField;
		}
		set
		{
			EventHeaderField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 1)]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public DateTime StartTime
	{
		get
		{
			return StartTimeField;
		}
		set
		{
			if (value.Kind == DateTimeKind.Unspecified)
			{
				value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
			}
			StartTimeField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 3)]
	public DateTime EndTime
	{
		get
		{
			return EndTimeField;
		}
		set
		{
			if (value.Kind == DateTimeKind.Unspecified)
			{
				value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
			}
			EndTimeField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 4)]
	public string Location
	{
		get
		{
			return LocationField;
		}
		set
		{
			LocationField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 5)]
	public ushort State
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

	[DataMember(EmitDefaultValue = false, Order = 6)]
	public string StateName
	{
		get
		{
			return StateNameField;
		}
		set
		{
			StateNameField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 7)]
	public ushort Category
	{
		get
		{
			return CategoryField;
		}
		set
		{
			CategoryField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 8)]
	public string CategoryName
	{
		get
		{
			return CategoryNameField;
		}
		set
		{
			CategoryNameField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 9)]
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

	[DataMember(EmitDefaultValue = false, Order = 10)]
	public uint Count
	{
		get
		{
			return CountField;
		}
		set
		{
			CountField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 11)]
	public RuleList RuleList
	{
		get
		{
			return RuleListField;
		}
		set
		{
			RuleListField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 12)]
	public AnalyticsObjectList ObjectList
	{
		get
		{
			return ObjectListField;
		}
		set
		{
			ObjectListField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 13)]
	public ReferenceList ReferenceList
	{
		get
		{
			return ReferenceListField;
		}
		set
		{
			ReferenceListField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 14)]
	public SnapshotList SnapshotList
	{
		get
		{
			return SnapshotListField;
		}
		set
		{
			SnapshotListField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 15)]
	public Vendor Vendor
	{
		get
		{
			return VendorField;
		}
		set
		{
			VendorField = value;
		}
	}
}
