using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Condition", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
[KnownType(typeof(AlarmServiceFault))]
[KnownType(typeof(AlarmFaultType))]
[KnownType(typeof(AlarmFilter))]
[KnownType(typeof(EventFilter))]
[KnownType(typeof(Condition[]))]
[KnownType(typeof(Operator))]
[KnownType(typeof(Target))]
[KnownType(typeof(OrderBy[]))]
[KnownType(typeof(OrderBy))]
[KnownType(typeof(Order))]
[KnownType(typeof(AlarmLine[]))]
[KnownType(typeof(AlarmLine))]
[KnownType(typeof(EventLine[]))]
[KnownType(typeof(EventLine))]
[KnownType(typeof(Statistic[]))]
[KnownType(typeof(Statistic))]
[KnownType(typeof(StatisticType))]
[KnownType(typeof(AlarmHistory[]))]
[KnownType(typeof(AlarmHistory))]
[KnownType(typeof(VideoOS.Platform.Data.Alarm))]
[KnownType(typeof(EventHeader))]
[KnownType(typeof(EventSource))]
[KnownType(typeof(FQID))]
[KnownType(typeof(ServerId))]
[KnownType(typeof(FolderType))]
[KnownType(typeof(RuleList))]
[KnownType(typeof(Rule))]
[KnownType(typeof(PolygonList))]
[KnownType(typeof(TPolygon))]
[KnownType(typeof(TColor))]
[KnownType(typeof(PointList))]
[KnownType(typeof(TPoint))]
[KnownType(typeof(AnalyticsObjectList))]
[KnownType(typeof(AnalyticsObject))]
[KnownType(typeof(BoundingBox))]
[KnownType(typeof(ObjectMotion))]
[KnownType(typeof(ReferenceList))]
[KnownType(typeof(Reference))]
[KnownType(typeof(SnapshotList))]
[KnownType(typeof(Snapshot))]
[KnownType(typeof(Vendor))]
[KnownType(typeof(string[]))]
public class Condition : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Operator OperatorField;

	private Target TargetField;

	private object ValueField;

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
	public Operator Operator
	{
		get
		{
			return OperatorField;
		}
		set
		{
			OperatorField = value;
		}
	}

	[DataMember]
	public Target Target
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
	public object Value
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
}
