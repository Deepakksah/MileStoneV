using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AnalyticsObject", Namespace = "urn:milestone-systems")]
public class AnalyticsObject : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid IDField;

	private string NameField;

	private string TypeField;

	private string DescriptionField;

	private double ConfidenceField;

	private string ValueField;

	private string DataField;

	private bool AlarmTriggerField;

	private bool RemovedField;

	private string ColorField;

	private double SizeField;

	private string SizeUnitField;

	private BoundingBox BoundingBoxField;

	private TPolygon PolygonField;

	private ObjectMotion MotionField;

	private byte[] MaskField;

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

	[DataMember(EmitDefaultValue = false)]
	public Guid ID
	{
		get
		{
			return IDField;
		}
		set
		{
			IDField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
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

	[DataMember(EmitDefaultValue = false, Order = 3)]
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

	[DataMember(EmitDefaultValue = false, Order = 4)]
	public double Confidence
	{
		get
		{
			return ConfidenceField;
		}
		set
		{
			ConfidenceField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 5)]
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

	[DataMember(EmitDefaultValue = false, Order = 6)]
	public bool AlarmTrigger
	{
		get
		{
			return AlarmTriggerField;
		}
		set
		{
			AlarmTriggerField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 7)]
	public bool Removed
	{
		get
		{
			return RemovedField;
		}
		set
		{
			RemovedField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 8)]
	public string Color
	{
		get
		{
			return ColorField;
		}
		set
		{
			ColorField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 9)]
	public double Size
	{
		get
		{
			return SizeField;
		}
		set
		{
			SizeField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 10)]
	public string SizeUnit
	{
		get
		{
			return SizeUnitField;
		}
		set
		{
			SizeUnitField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 11)]
	public BoundingBox BoundingBox
	{
		get
		{
			return BoundingBoxField;
		}
		set
		{
			BoundingBoxField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 12)]
	public TPolygon Polygon
	{
		get
		{
			return PolygonField;
		}
		set
		{
			PolygonField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 13)]
	public ObjectMotion Motion
	{
		get
		{
			return MotionField;
		}
		set
		{
			MotionField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 14)]
	public byte[] Mask
	{
		get
		{
			return MaskField;
		}
		set
		{
			MaskField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 15)]
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
}
