using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace VideoOS.DataCollector.Service.Grpc;

public sealed class PerformanceCounterValue : IMessage<PerformanceCounterValue>, IMessage, IEquatable<PerformanceCounterValue>, IDeepCloneable<PerformanceCounterValue>, IBufferMessage
{
	private static readonly MessageParser<PerformanceCounterValue> _parser = new MessageParser<PerformanceCounterValue>(() => new PerformanceCounterValue());

	private UnknownFieldSet _unknownFields;

	public const int CategoryNameFieldNumber = 1;

	private string categoryName_ = "";

	public const int CounterNameFieldNumber = 2;

	private string counterName_ = "";

	public const int InstanceNameFieldNumber = 3;

	private string instanceName_ = "";

	public const int CollectionTimeFieldNumber = 4;

	private Timestamp collectionTime_;

	public const int RawValueFieldNumber = 5;

	private long rawValue_;

	public const int CalculatedValueFieldNumber = 6;

	private float calculatedValue_;

	public const int SendImmediatelyFieldNumber = 7;

	private bool sendImmediately_;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageParser<PerformanceCounterValue> Parser => _parser;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.MessageTypes[10];

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
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
	[GeneratedCode("protoc", null)]
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
	[GeneratedCode("protoc", null)]
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
	[GeneratedCode("protoc", null)]
	public Timestamp CollectionTime
	{
		get
		{
			return collectionTime_;
		}
		set
		{
			collectionTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public long RawValue
	{
		get
		{
			return rawValue_;
		}
		set
		{
			rawValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public float CalculatedValue
	{
		get
		{
			return calculatedValue_;
		}
		set
		{
			calculatedValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool SendImmediately
	{
		get
		{
			return sendImmediately_;
		}
		set
		{
			sendImmediately_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public PerformanceCounterValue()
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public PerformanceCounterValue(PerformanceCounterValue other)
		: this()
	{
		categoryName_ = other.categoryName_;
		counterName_ = other.counterName_;
		instanceName_ = other.instanceName_;
		collectionTime_ = ((other.collectionTime_ != null) ? other.collectionTime_.Clone() : null);
		rawValue_ = other.rawValue_;
		calculatedValue_ = other.calculatedValue_;
		sendImmediately_ = other.sendImmediately_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public PerformanceCounterValue Clone()
	{
		return new PerformanceCounterValue(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public override bool Equals(object other)
	{
		return Equals(other as PerformanceCounterValue);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool Equals(PerformanceCounterValue other)
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
		if (!object.Equals(CollectionTime, other.CollectionTime))
		{
			return false;
		}
		if (RawValue != other.RawValue)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(CalculatedValue, other.CalculatedValue))
		{
			return false;
		}
		if (SendImmediately != other.SendImmediately)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
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
		if (collectionTime_ != null)
		{
			num ^= CollectionTime.GetHashCode();
		}
		if (RawValue != 0L)
		{
			num ^= RawValue.GetHashCode();
		}
		if (CalculatedValue != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(CalculatedValue);
		}
		if (SendImmediately)
		{
			num ^= SendImmediately.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void WriteTo(CodedOutputStream output)
	{
		output.WriteRawMessage(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
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
		if (collectionTime_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(CollectionTime);
		}
		if (RawValue != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(RawValue);
		}
		if (CalculatedValue != 0f)
		{
			output.WriteRawTag(53);
			output.WriteFloat(CalculatedValue);
		}
		if (SendImmediately)
		{
			output.WriteRawTag(56);
			output.WriteBool(SendImmediately);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(ref output);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
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
		if (collectionTime_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(CollectionTime);
		}
		if (RawValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(RawValue);
		}
		if (CalculatedValue != 0f)
		{
			num += 5;
		}
		if (SendImmediately)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void MergeFrom(PerformanceCounterValue other)
	{
		if (other == null)
		{
			return;
		}
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
		if (other.collectionTime_ != null)
		{
			if (collectionTime_ == null)
			{
				CollectionTime = new Timestamp();
			}
			CollectionTime.MergeFrom(other.CollectionTime);
		}
		if (other.RawValue != 0L)
		{
			RawValue = other.RawValue;
		}
		if (other.CalculatedValue != 0f)
		{
			CalculatedValue = other.CalculatedValue;
		}
		if (other.SendImmediately)
		{
			SendImmediately = other.SendImmediately;
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void MergeFrom(CodedInputStream input)
	{
		input.ReadRawMessage(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
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
			case 34u:
				if (collectionTime_ == null)
				{
					CollectionTime = new Timestamp();
				}
				input.ReadMessage(CollectionTime);
				break;
			case 40u:
				RawValue = input.ReadInt64();
				break;
			case 53u:
				CalculatedValue = input.ReadFloat();
				break;
			case 56u:
				SendImmediately = input.ReadBool();
				break;
			}
		}
	}
}
