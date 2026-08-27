using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public sealed class PerformanceCounterInstance : IMessage<PerformanceCounterInstance>, IMessage, IEquatable<PerformanceCounterInstance>, IDeepCloneable<PerformanceCounterInstance>, IBufferMessage
{
	private static readonly MessageParser<PerformanceCounterInstance> _parser = new MessageParser<PerformanceCounterInstance>(() => new PerformanceCounterInstance());

	private UnknownFieldSet _unknownFields;

	public const int CategoryNameFieldNumber = 1;

	private string categoryName_ = "";

	public const int CounterNameFieldNumber = 2;

	private string counterName_ = "";

	public const int InstanceNameFieldNumber = 3;

	private string instanceName_ = "";

	public const int CounterTypeFieldNumber = 4;

	private PerformanceCounterType counterType_;

	public const int CounterDirectionFieldNumber = 5;

	private bool counterDirection_;

	public const int EnableHistoryFieldNumber = 6;

	private bool enableHistory_;

	public const int TriggerStatePerValueFieldNumber = 7;

	private bool triggerStatePerValue_;

	public const int CategoryTypeFieldNumber = 8;

	private PerformanceCounterCategoryType categoryType_;

	public const int SampleFrequencyFieldNumber = 9;

	private int sampleFrequency_;

	public const int PathFieldNumber = 10;

	private string path_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<PerformanceCounterInstance> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public string InstanceName
	{
		get
		{
			return instanceName_;
		}
		set
		{
			instanceName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public PerformanceCounterType CounterType
	{
		get
		{
			return counterType_;
		}
		set
		{
			counterType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool CounterDirection
	{
		get
		{
			return counterDirection_;
		}
		set
		{
			counterDirection_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool EnableHistory
	{
		get
		{
			return enableHistory_;
		}
		set
		{
			enableHistory_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool TriggerStatePerValue
	{
		get
		{
			return triggerStatePerValue_;
		}
		set
		{
			triggerStatePerValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public PerformanceCounterCategoryType CategoryType
	{
		get
		{
			return categoryType_;
		}
		set
		{
			categoryType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SampleFrequency
	{
		get
		{
			return sampleFrequency_;
		}
		set
		{
			sampleFrequency_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Path
	{
		get
		{
			return path_;
		}
		set
		{
			path_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public PerformanceCounterInstance()
	{
	}

	[DebuggerNonUserCode]
	public PerformanceCounterInstance(PerformanceCounterInstance other)
		: this()
	{
		categoryName_ = other.categoryName_;
		counterName_ = other.counterName_;
		instanceName_ = other.instanceName_;
		counterType_ = other.counterType_;
		counterDirection_ = other.counterDirection_;
		enableHistory_ = other.enableHistory_;
		triggerStatePerValue_ = other.triggerStatePerValue_;
		categoryType_ = other.categoryType_;
		sampleFrequency_ = other.sampleFrequency_;
		path_ = other.path_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PerformanceCounterInstance Clone()
	{
		return new PerformanceCounterInstance(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PerformanceCounterInstance);
	}

	[DebuggerNonUserCode]
	public bool Equals(PerformanceCounterInstance other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CategoryName != other.CategoryName)
		{
			return false;
		}
		if (CounterName != other.CounterName)
		{
			return false;
		}
		if (InstanceName != other.InstanceName)
		{
			return false;
		}
		if (CounterType != other.CounterType)
		{
			return false;
		}
		if (CounterDirection != other.CounterDirection)
		{
			return false;
		}
		if (EnableHistory != other.EnableHistory)
		{
			return false;
		}
		if (TriggerStatePerValue != other.TriggerStatePerValue)
		{
			return false;
		}
		if (CategoryType != other.CategoryType)
		{
			return false;
		}
		if (SampleFrequency != other.SampleFrequency)
		{
			return false;
		}
		if (Path != other.Path)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CategoryName.Length != 0)
		{
			num ^= CategoryName.GetHashCode();
		}
		if (CounterName.Length != 0)
		{
			num ^= CounterName.GetHashCode();
		}
		if (InstanceName.Length != 0)
		{
			num ^= InstanceName.GetHashCode();
		}
		if (CounterType != PerformanceCounterType.NumberOfItemsHex32)
		{
			num ^= CounterType.GetHashCode();
		}
		if (CounterDirection)
		{
			num ^= CounterDirection.GetHashCode();
		}
		if (EnableHistory)
		{
			num ^= EnableHistory.GetHashCode();
		}
		if (TriggerStatePerValue)
		{
			num ^= TriggerStatePerValue.GetHashCode();
		}
		if (CategoryType != PerformanceCounterCategoryType.SingleInstance)
		{
			num ^= CategoryType.GetHashCode();
		}
		if (SampleFrequency != 0)
		{
			num ^= SampleFrequency.GetHashCode();
		}
		if (Path.Length != 0)
		{
			num ^= Path.GetHashCode();
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
		if (CategoryName.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(CategoryName);
		}
		if (CounterName.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(CounterName);
		}
		if (InstanceName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(InstanceName);
		}
		if (CounterType != PerformanceCounterType.NumberOfItemsHex32)
		{
			output.WriteRawTag(32);
			output.WriteEnum((int)CounterType);
		}
		if (CounterDirection)
		{
			output.WriteRawTag(40);
			output.WriteBool(CounterDirection);
		}
		if (EnableHistory)
		{
			output.WriteRawTag(48);
			output.WriteBool(EnableHistory);
		}
		if (TriggerStatePerValue)
		{
			output.WriteRawTag(56);
			output.WriteBool(TriggerStatePerValue);
		}
		if (CategoryType != PerformanceCounterCategoryType.SingleInstance)
		{
			output.WriteRawTag(64);
			output.WriteEnum((int)CategoryType);
		}
		if (SampleFrequency != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(SampleFrequency);
		}
		if (Path.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(Path);
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
		if (CategoryName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CategoryName);
		}
		if (CounterName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CounterName);
		}
		if (InstanceName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(InstanceName);
		}
		if (CounterType != PerformanceCounterType.NumberOfItemsHex32)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)CounterType);
		}
		if (CounterDirection)
		{
			num += 2;
		}
		if (EnableHistory)
		{
			num += 2;
		}
		if (TriggerStatePerValue)
		{
			num += 2;
		}
		if (CategoryType != PerformanceCounterCategoryType.SingleInstance)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)CategoryType);
		}
		if (SampleFrequency != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SampleFrequency);
		}
		if (Path.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Path);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PerformanceCounterInstance other)
	{
		if (other != null)
		{
			if (other.CategoryName.Length != 0)
			{
				CategoryName = other.CategoryName;
			}
			if (other.CounterName.Length != 0)
			{
				CounterName = other.CounterName;
			}
			if (other.InstanceName.Length != 0)
			{
				InstanceName = other.InstanceName;
			}
			if (other.CounterType != PerformanceCounterType.NumberOfItemsHex32)
			{
				CounterType = other.CounterType;
			}
			if (other.CounterDirection)
			{
				CounterDirection = other.CounterDirection;
			}
			if (other.EnableHistory)
			{
				EnableHistory = other.EnableHistory;
			}
			if (other.TriggerStatePerValue)
			{
				TriggerStatePerValue = other.TriggerStatePerValue;
			}
			if (other.CategoryType != PerformanceCounterCategoryType.SingleInstance)
			{
				CategoryType = other.CategoryType;
			}
			if (other.SampleFrequency != 0)
			{
				SampleFrequency = other.SampleFrequency;
			}
			if (other.Path.Length != 0)
			{
				Path = other.Path;
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
				CategoryName = input.ReadString();
				break;
			case 18u:
				CounterName = input.ReadString();
				break;
			case 26u:
				InstanceName = input.ReadString();
				break;
			case 32u:
				CounterType = (PerformanceCounterType)input.ReadEnum();
				break;
			case 40u:
				CounterDirection = input.ReadBool();
				break;
			case 48u:
				EnableHistory = input.ReadBool();
				break;
			case 56u:
				TriggerStatePerValue = input.ReadBool();
				break;
			case 64u:
				CategoryType = (PerformanceCounterCategoryType)input.ReadEnum();
				break;
			case 72u:
				SampleFrequency = input.ReadInt32();
				break;
			case 82u:
				Path = input.ReadString();
				break;
			}
		}
	}
}
