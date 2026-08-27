using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public sealed class GetPerformanceCounterValuesRequest : IMessage<GetPerformanceCounterValuesRequest>, IMessage, IEquatable<GetPerformanceCounterValuesRequest>, IDeepCloneable<GetPerformanceCounterValuesRequest>, IBufferMessage
{
	private static readonly MessageParser<GetPerformanceCounterValuesRequest> _parser = new MessageParser<GetPerformanceCounterValuesRequest>(() => new GetPerformanceCounterValuesRequest());

	private UnknownFieldSet _unknownFields;

	public const int CategoryNameFieldNumber = 1;

	private string categoryName_ = "";

	public const int CounterNameFieldNumber = 2;

	private string counterName_ = "";

	public const int InstanceNameFieldNumber = 3;

	private string instanceName_ = "";

	public const int SampleFrequencyFieldNumber = 4;

	private int sampleFrequency_;

	[DebuggerNonUserCode]
	public static MessageParser<GetPerformanceCounterValuesRequest> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.MessageTypes[1];

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
	public GetPerformanceCounterValuesRequest()
	{
	}

	[DebuggerNonUserCode]
	public GetPerformanceCounterValuesRequest(GetPerformanceCounterValuesRequest other)
		: this()
	{
		categoryName_ = other.categoryName_;
		counterName_ = other.counterName_;
		instanceName_ = other.instanceName_;
		sampleFrequency_ = other.sampleFrequency_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GetPerformanceCounterValuesRequest Clone()
	{
		return new GetPerformanceCounterValuesRequest(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GetPerformanceCounterValuesRequest);
	}

	[DebuggerNonUserCode]
	public bool Equals(GetPerformanceCounterValuesRequest other)
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
		if (SampleFrequency != other.SampleFrequency)
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
		if (SampleFrequency != 0)
		{
			num ^= SampleFrequency.GetHashCode();
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
		if (SampleFrequency != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(SampleFrequency);
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
		if (SampleFrequency != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SampleFrequency);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GetPerformanceCounterValuesRequest other)
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
			if (other.SampleFrequency != 0)
			{
				SampleFrequency = other.SampleFrequency;
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
				SampleFrequency = input.ReadInt32();
				break;
			}
		}
	}
}
