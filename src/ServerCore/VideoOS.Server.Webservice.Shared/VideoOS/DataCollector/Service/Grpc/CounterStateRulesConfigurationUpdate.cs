using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public sealed class CounterStateRulesConfigurationUpdate : IMessage<CounterStateRulesConfigurationUpdate>, IMessage, IEquatable<CounterStateRulesConfigurationUpdate>, IDeepCloneable<CounterStateRulesConfigurationUpdate>, IBufferMessage
{
	private static readonly MessageParser<CounterStateRulesConfigurationUpdate> _parser = new MessageParser<CounterStateRulesConfigurationUpdate>(() => new CounterStateRulesConfigurationUpdate());

	private UnknownFieldSet _unknownFields;

	public const int DeletedFieldNumber = 1;

	private bool deleted_;

	public const int RuleConfigFieldNumber = 2;

	private CounterStateRuleConfiguration ruleConfig_;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageParser<CounterStateRulesConfigurationUpdate> Parser => _parser;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.MessageTypes[8];

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool Deleted
	{
		get
		{
			return deleted_;
		}
		set
		{
			deleted_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public CounterStateRuleConfiguration RuleConfig
	{
		get
		{
			return ruleConfig_;
		}
		set
		{
			ruleConfig_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public CounterStateRulesConfigurationUpdate()
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public CounterStateRulesConfigurationUpdate(CounterStateRulesConfigurationUpdate other)
		: this()
	{
		deleted_ = other.deleted_;
		ruleConfig_ = ((other.ruleConfig_ != null) ? other.ruleConfig_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public CounterStateRulesConfigurationUpdate Clone()
	{
		return new CounterStateRulesConfigurationUpdate(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public override bool Equals(object other)
	{
		return Equals(other as CounterStateRulesConfigurationUpdate);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool Equals(CounterStateRulesConfigurationUpdate other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Deleted != other.Deleted)
		{
			return false;
		}
		if (!object.Equals(RuleConfig, other.RuleConfig))
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
		if (Deleted)
		{
			num ^= Deleted.GetHashCode();
		}
		if (ruleConfig_ != null)
		{
			num ^= RuleConfig.GetHashCode();
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
		if (Deleted)
		{
			output.WriteRawTag(8);
			output.WriteBool(Deleted);
		}
		if (ruleConfig_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(RuleConfig);
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
		if (Deleted)
		{
			num += 2;
		}
		if (ruleConfig_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(RuleConfig);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void MergeFrom(CounterStateRulesConfigurationUpdate other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Deleted)
		{
			Deleted = other.Deleted;
		}
		if (other.ruleConfig_ != null)
		{
			if (ruleConfig_ == null)
			{
				RuleConfig = new CounterStateRuleConfiguration();
			}
			RuleConfig.MergeFrom(other.RuleConfig);
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
			case 8u:
				Deleted = input.ReadBool();
				break;
			case 18u:
				if (ruleConfig_ == null)
				{
					RuleConfig = new CounterStateRuleConfiguration();
				}
				input.ReadMessage(RuleConfig);
				break;
			}
		}
	}
}
