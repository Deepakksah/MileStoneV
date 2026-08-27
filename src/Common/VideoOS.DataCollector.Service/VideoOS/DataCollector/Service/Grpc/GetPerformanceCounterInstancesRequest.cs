using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public sealed class GetPerformanceCounterInstancesRequest : IMessage<GetPerformanceCounterInstancesRequest>, IMessage, IEquatable<GetPerformanceCounterInstancesRequest>, IDeepCloneable<GetPerformanceCounterInstancesRequest>, IBufferMessage
{
	private static readonly MessageParser<GetPerformanceCounterInstancesRequest> _parser = new MessageParser<GetPerformanceCounterInstancesRequest>(() => new GetPerformanceCounterInstancesRequest());

	private UnknownFieldSet _unknownFields;

	public const int CategoryNameFieldNumber = 1;

	private string categoryName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GetPerformanceCounterInstancesRequest> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.MessageTypes[0];

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
	public GetPerformanceCounterInstancesRequest()
	{
	}

	[DebuggerNonUserCode]
	public GetPerformanceCounterInstancesRequest(GetPerformanceCounterInstancesRequest other)
		: this()
	{
		categoryName_ = other.categoryName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GetPerformanceCounterInstancesRequest Clone()
	{
		return new GetPerformanceCounterInstancesRequest(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GetPerformanceCounterInstancesRequest);
	}

	[DebuggerNonUserCode]
	public bool Equals(GetPerformanceCounterInstancesRequest other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GetPerformanceCounterInstancesRequest other)
	{
		if (other != null)
		{
			if (other.CategoryName.Length != 0)
			{
				CategoryName = other.CategoryName;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
			}
			else
			{
				CategoryName = input.ReadString();
			}
		}
	}
}
