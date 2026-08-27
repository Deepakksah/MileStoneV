using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public sealed class GetPerformanceCounterValuesResponse : IMessage<GetPerformanceCounterValuesResponse>, IMessage, IEquatable<GetPerformanceCounterValuesResponse>, IDeepCloneable<GetPerformanceCounterValuesResponse>, IBufferMessage
{
	private static readonly MessageParser<GetPerformanceCounterValuesResponse> _parser = new MessageParser<GetPerformanceCounterValuesResponse>(() => new GetPerformanceCounterValuesResponse());

	private UnknownFieldSet _unknownFields;

	public const int ValuesFieldNumber = 1;

	private static readonly FieldCodec<PerformanceCounterValue> _repeated_values_codec = FieldCodec.ForMessage(10u, PerformanceCounterValue.Parser);

	private readonly RepeatedField<PerformanceCounterValue> values_ = new RepeatedField<PerformanceCounterValue>();

	[DebuggerNonUserCode]
	public static MessageParser<GetPerformanceCounterValuesResponse> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<PerformanceCounterValue> Values => values_;

	[DebuggerNonUserCode]
	public GetPerformanceCounterValuesResponse()
	{
	}

	[DebuggerNonUserCode]
	public GetPerformanceCounterValuesResponse(GetPerformanceCounterValuesResponse other)
		: this()
	{
		values_ = other.values_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GetPerformanceCounterValuesResponse Clone()
	{
		return new GetPerformanceCounterValuesResponse(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GetPerformanceCounterValuesResponse);
	}

	[DebuggerNonUserCode]
	public bool Equals(GetPerformanceCounterValuesResponse other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!values_.Equals(other.values_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= values_.GetHashCode();
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
		values_.WriteTo(ref output, _repeated_values_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(ref output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += values_.CalculateSize(_repeated_values_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GetPerformanceCounterValuesResponse other)
	{
		if (other != null)
		{
			values_.Add(other.values_);
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
				values_.AddEntriesFrom(ref input, _repeated_values_codec);
			}
		}
	}
}
