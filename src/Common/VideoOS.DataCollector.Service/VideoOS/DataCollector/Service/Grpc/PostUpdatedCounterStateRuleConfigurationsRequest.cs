using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public sealed class PostUpdatedCounterStateRuleConfigurationsRequest : IMessage<PostUpdatedCounterStateRuleConfigurationsRequest>, IMessage, IEquatable<PostUpdatedCounterStateRuleConfigurationsRequest>, IDeepCloneable<PostUpdatedCounterStateRuleConfigurationsRequest>, IBufferMessage
{
	private static readonly MessageParser<PostUpdatedCounterStateRuleConfigurationsRequest> _parser = new MessageParser<PostUpdatedCounterStateRuleConfigurationsRequest>(() => new PostUpdatedCounterStateRuleConfigurationsRequest());

	private UnknownFieldSet _unknownFields;

	public const int RuleConfigUpdatesFieldNumber = 1;

	private static readonly FieldCodec<CounterStateRulesConfigurationUpdate> _repeated_ruleConfigUpdates_codec = FieldCodec.ForMessage(10u, CounterStateRulesConfigurationUpdate.Parser);

	private readonly RepeatedField<CounterStateRulesConfigurationUpdate> ruleConfigUpdates_ = new RepeatedField<CounterStateRulesConfigurationUpdate>();

	[DebuggerNonUserCode]
	public static MessageParser<PostUpdatedCounterStateRuleConfigurationsRequest> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<CounterStateRulesConfigurationUpdate> RuleConfigUpdates => ruleConfigUpdates_;

	[DebuggerNonUserCode]
	public PostUpdatedCounterStateRuleConfigurationsRequest()
	{
	}

	[DebuggerNonUserCode]
	public PostUpdatedCounterStateRuleConfigurationsRequest(PostUpdatedCounterStateRuleConfigurationsRequest other)
		: this()
	{
		ruleConfigUpdates_ = other.ruleConfigUpdates_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PostUpdatedCounterStateRuleConfigurationsRequest Clone()
	{
		return new PostUpdatedCounterStateRuleConfigurationsRequest(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PostUpdatedCounterStateRuleConfigurationsRequest);
	}

	[DebuggerNonUserCode]
	public bool Equals(PostUpdatedCounterStateRuleConfigurationsRequest other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!ruleConfigUpdates_.Equals(other.ruleConfigUpdates_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= ruleConfigUpdates_.GetHashCode();
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
		ruleConfigUpdates_.WriteTo(ref output, _repeated_ruleConfigUpdates_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(ref output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += ruleConfigUpdates_.CalculateSize(_repeated_ruleConfigUpdates_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PostUpdatedCounterStateRuleConfigurationsRequest other)
	{
		if (other != null)
		{
			ruleConfigUpdates_.Add(other.ruleConfigUpdates_);
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
				ruleConfigUpdates_.AddEntriesFrom(ref input, _repeated_ruleConfigUpdates_codec);
			}
		}
	}
}
