using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public sealed class CounterStateRuleConfiguration : IMessage<CounterStateRuleConfiguration>, IMessage, IEquatable<CounterStateRuleConfiguration>, IDeepCloneable<CounterStateRuleConfiguration>, IBufferMessage
{
	private static readonly MessageParser<CounterStateRuleConfiguration> _parser = new MessageParser<CounterStateRuleConfiguration>(() => new CounterStateRuleConfiguration());

	private UnknownFieldSet _unknownFields;

	public const int CounterPathFieldNumber = 1;

	private string counterPath_ = "";

	public const int WarningThresholdFieldNumber = 2;

	private float warningThreshold_;

	public const int CriticalThresholdFieldNumber = 3;

	private float criticalThreshold_;

	public const int TimeWindowFieldNumber = 4;

	private int timeWindow_;

	public const int CounterRuleIdFieldNumber = 5;

	private string counterRuleId_ = "";

	public const int NameFieldNumber = 6;

	private string name_ = "";

	public const int CategoryNameFieldNumber = 7;

	private string categoryName_ = "";

	public const int CounterNameFieldNumber = 8;

	private string counterName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<CounterStateRuleConfiguration> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.MessageTypes[11];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string CounterPath
	{
		get
		{
			return counterPath_;
		}
		set
		{
			counterPath_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public float WarningThreshold
	{
		get
		{
			return warningThreshold_;
		}
		set
		{
			warningThreshold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float CriticalThreshold
	{
		get
		{
			return criticalThreshold_;
		}
		set
		{
			criticalThreshold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TimeWindow
	{
		get
		{
			return timeWindow_;
		}
		set
		{
			timeWindow_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string CounterRuleId
	{
		get
		{
			return counterRuleId_;
		}
		set
		{
			counterRuleId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string CategoryName
	{
		get
		{
			return categoryName_;
		}
		set
		{
			categoryName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string CounterName
	{
		get
		{
			return counterName_;
		}
		set
		{
			counterName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public CounterStateRuleConfiguration()
	{
	}

	[DebuggerNonUserCode]
	public CounterStateRuleConfiguration(CounterStateRuleConfiguration other)
		: this()
	{
		counterPath_ = other.counterPath_;
		warningThreshold_ = other.warningThreshold_;
		criticalThreshold_ = other.criticalThreshold_;
		timeWindow_ = other.timeWindow_;
		counterRuleId_ = other.counterRuleId_;
		name_ = other.name_;
		categoryName_ = other.categoryName_;
		counterName_ = other.counterName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CounterStateRuleConfiguration Clone()
	{
		return new CounterStateRuleConfiguration(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CounterStateRuleConfiguration);
	}

	[DebuggerNonUserCode]
	public bool Equals(CounterStateRuleConfiguration other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CounterPath != other.CounterPath)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(WarningThreshold, other.WarningThreshold))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(CriticalThreshold, other.CriticalThreshold))
		{
			return false;
		}
		if (TimeWindow != other.TimeWindow)
		{
			return false;
		}
		if (CounterRuleId != other.CounterRuleId)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (CategoryName != other.CategoryName)
		{
			return false;
		}
		if (CounterName != other.CounterName)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CounterPath.Length != 0)
		{
			num ^= CounterPath.GetHashCode();
		}
		if (WarningThreshold != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(WarningThreshold);
		}
		if (CriticalThreshold != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(CriticalThreshold);
		}
		if (TimeWindow != 0)
		{
			num ^= TimeWindow.GetHashCode();
		}
		if (CounterRuleId.Length != 0)
		{
			num ^= CounterRuleId.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (CategoryName.Length != 0)
		{
			num ^= CategoryName.GetHashCode();
		}
		if (CounterName.Length != 0)
		{
			num ^= CounterName.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		output.WriteRawMessage(this);
	}

	[DebuggerNonUserCode]
	void IBufferMessage.InternalWriteTo(ref WriteContext output)
	{
		if (CounterPath.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(CounterPath);
		}
		if (WarningThreshold != 0f)
		{
			output.WriteRawTag(21);
			output.WriteFloat(WarningThreshold);
		}
		if (CriticalThreshold != 0f)
		{
			output.WriteRawTag(29);
			output.WriteFloat(CriticalThreshold);
		}
		if (TimeWindow != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(TimeWindow);
		}
		if (CounterRuleId.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(CounterRuleId);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(Name);
		}
		if (CategoryName.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(CategoryName);
		}
		if (CounterName.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(CounterName);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(ref output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (CounterPath.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CounterPath);
		}
		if (WarningThreshold != 0f)
		{
			num += 5;
		}
		if (CriticalThreshold != 0f)
		{
			num += 5;
		}
		if (TimeWindow != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TimeWindow);
		}
		if (CounterRuleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CounterRuleId);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (CategoryName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CategoryName);
		}
		if (CounterName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CounterName);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CounterStateRuleConfiguration other)
	{
		if (other != null)
		{
			if (other.CounterPath.Length != 0)
			{
				CounterPath = other.CounterPath;
			}
			if (other.WarningThreshold != 0f)
			{
				WarningThreshold = other.WarningThreshold;
			}
			if (other.CriticalThreshold != 0f)
			{
				CriticalThreshold = other.CriticalThreshold;
			}
			if (other.TimeWindow != 0)
			{
				TimeWindow = other.TimeWindow;
			}
			if (other.CounterRuleId.Length != 0)
			{
				CounterRuleId = other.CounterRuleId;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.CategoryName.Length != 0)
			{
				CategoryName = other.CategoryName;
			}
			if (other.CounterName.Length != 0)
			{
				CounterName = other.CounterName;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		input.ReadRawMessage(this);
	}

	[DebuggerNonUserCode]
	void IBufferMessage.InternalMergeFrom(ref ParseContext input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
				break;
			case 10u:
				CounterPath = input.ReadString();
				break;
			case 21u:
				WarningThreshold = input.ReadFloat();
				break;
			case 29u:
				CriticalThreshold = input.ReadFloat();
				break;
			case 32u:
				TimeWindow = input.ReadInt32();
				break;
			case 42u:
				CounterRuleId = input.ReadString();
				break;
			case 50u:
				Name = input.ReadString();
				break;
			case 58u:
				CategoryName = input.ReadString();
				break;
			case 66u:
				CounterName = input.ReadString();
				break;
			}
		}
	}
}
