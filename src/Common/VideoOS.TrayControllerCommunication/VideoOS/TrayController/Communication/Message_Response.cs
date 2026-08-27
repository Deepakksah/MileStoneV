using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace VideoOS.TrayController.Communication;

[DebuggerDisplay("{ToString(),nq}")]
public sealed class Message_Response : IMessage<Message_Response>, IMessage, IEquatable<Message_Response>, IDeepCloneable<Message_Response>, IBufferMessage
{
	private static readonly MessageParser<Message_Response> _parser = new MessageParser<Message_Response>((Func<Message_Response>)(() => new Message_Response()));

	private UnknownFieldSet _unknownFields;

	public const int SequenceFieldNumber = 1;

	private long sequence_;

	public const int WhenFieldNumber = 2;

	private Timestamp when_;

	public const int WhatFieldNumber = 3;

	private string what_ = "";

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageParser<Message_Response> Parser => _parser;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageDescriptor Descriptor => TrayControllerReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public long Sequence
	{
		get
		{
			return sequence_;
		}
		set
		{
			sequence_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public Timestamp When
	{
		get
		{
			return when_;
		}
		set
		{
			when_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public string What
	{
		get
		{
			return what_;
		}
		set
		{
			what_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public Message_Response()
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public Message_Response(Message_Response other)
		: this()
	{
		sequence_ = other.sequence_;
		when_ = ((other.when_ != (Timestamp)null) ? other.when_.Clone() : null);
		what_ = other.what_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public Message_Response Clone()
	{
		return new Message_Response(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public override bool Equals(object other)
	{
		return Equals(other as Message_Response);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool Equals(Message_Response other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Sequence != other.Sequence)
		{
			return false;
		}
		if (!object.Equals(When, other.When))
		{
			return false;
		}
		if (What != other.What)
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
		if (Sequence != 0L)
		{
			num ^= Sequence.GetHashCode();
		}
		if (when_ != (Timestamp)null)
		{
			num ^= ((object)When).GetHashCode();
		}
		if (What.Length != 0)
		{
			num ^= What.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= ((object)_unknownFields).GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString((IMessage)(object)this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void WriteTo(CodedOutputStream output)
	{
		output.WriteRawMessage((IMessage)(object)this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	void IBufferMessage.InternalWriteTo(ref WriteContext output)
	{
		if (Sequence != 0L)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)8);
			((WriteContext)(ref output)).WriteInt64(Sequence);
		}
		if (when_ != (Timestamp)null)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)18);
			((WriteContext)(ref output)).WriteMessage((IMessage)(object)When);
		}
		if (What.Length != 0)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)26);
			((WriteContext)(ref output)).WriteString(What);
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
		if (Sequence != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Sequence);
		}
		if (when_ != (Timestamp)null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize((IMessage)(object)When);
		}
		if (What.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(What);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void MergeFrom(Message_Response other)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		if (other == null)
		{
			return;
		}
		if (other.Sequence != 0L)
		{
			Sequence = other.Sequence;
		}
		if (other.when_ != (Timestamp)null)
		{
			if (when_ == (Timestamp)null)
			{
				When = new Timestamp();
			}
			When.MergeFrom(other.When);
		}
		if (other.What.Length != 0)
		{
			What = other.What;
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void MergeFrom(CodedInputStream input)
	{
		input.ReadRawMessage((IMessage)(object)this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	void IBufferMessage.InternalMergeFrom(ref ParseContext input)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		uint num;
		while ((num = ((ParseContext)(ref input)).ReadTag()) != 0 && (num & 7) != 4)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
				break;
			case 8u:
				Sequence = ((ParseContext)(ref input)).ReadInt64();
				break;
			case 18u:
				if (when_ == (Timestamp)null)
				{
					When = new Timestamp();
				}
				((ParseContext)(ref input)).ReadMessage((IMessage)(object)When);
				break;
			case 26u:
				What = ((ParseContext)(ref input)).ReadString();
				break;
			}
		}
	}
}
