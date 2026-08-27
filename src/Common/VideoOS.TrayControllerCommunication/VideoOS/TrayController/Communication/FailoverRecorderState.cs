using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace VideoOS.TrayController.Communication;

[DebuggerDisplay("{ToString(),nq}")]
public sealed class FailoverRecorderState : IMessage<FailoverRecorderState>, IMessage, IEquatable<FailoverRecorderState>, IDeepCloneable<FailoverRecorderState>, IBufferMessage
{
	private static readonly MessageParser<FailoverRecorderState> _parser = new MessageParser<FailoverRecorderState>((Func<FailoverRecorderState>)(() => new FailoverRecorderState()));

	private UnknownFieldSet _unknownFields;

	public const int HasSslErrorWithLocalRecorderFieldNumber = 1;

	private bool hasSslErrorWithLocalRecorder_;

	public const int HasAuthorizationErrorWithLocalRecorderFieldNumber = 2;

	private bool hasAuthorizationErrorWithLocalRecorder_;

	public const int LocalRecorderStartupErrorFieldNumber = 3;

	private bool localRecorderStartupError_;

	public const int WatchItemErrorFieldNumber = 4;

	private RecorderWatchItemError watchItemError_;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageParser<FailoverRecorderState> Parser => _parser;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageDescriptor Descriptor => TrayControllerReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool HasSslErrorWithLocalRecorder
	{
		get
		{
			return hasSslErrorWithLocalRecorder_;
		}
		set
		{
			hasSslErrorWithLocalRecorder_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool HasAuthorizationErrorWithLocalRecorder
	{
		get
		{
			return hasAuthorizationErrorWithLocalRecorder_;
		}
		set
		{
			hasAuthorizationErrorWithLocalRecorder_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool LocalRecorderStartupError
	{
		get
		{
			return localRecorderStartupError_;
		}
		set
		{
			localRecorderStartupError_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public RecorderWatchItemError WatchItemError
	{
		get
		{
			return watchItemError_;
		}
		set
		{
			watchItemError_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public FailoverRecorderState()
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public FailoverRecorderState(FailoverRecorderState other)
		: this()
	{
		hasSslErrorWithLocalRecorder_ = other.hasSslErrorWithLocalRecorder_;
		hasAuthorizationErrorWithLocalRecorder_ = other.hasAuthorizationErrorWithLocalRecorder_;
		localRecorderStartupError_ = other.localRecorderStartupError_;
		watchItemError_ = other.watchItemError_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public FailoverRecorderState Clone()
	{
		return new FailoverRecorderState(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public override bool Equals(object other)
	{
		return Equals(other as FailoverRecorderState);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool Equals(FailoverRecorderState other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (HasSslErrorWithLocalRecorder != other.HasSslErrorWithLocalRecorder)
		{
			return false;
		}
		if (HasAuthorizationErrorWithLocalRecorder != other.HasAuthorizationErrorWithLocalRecorder)
		{
			return false;
		}
		if (LocalRecorderStartupError != other.LocalRecorderStartupError)
		{
			return false;
		}
		if (WatchItemError != other.WatchItemError)
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
		if (HasSslErrorWithLocalRecorder)
		{
			num ^= HasSslErrorWithLocalRecorder.GetHashCode();
		}
		if (HasAuthorizationErrorWithLocalRecorder)
		{
			num ^= HasAuthorizationErrorWithLocalRecorder.GetHashCode();
		}
		if (LocalRecorderStartupError)
		{
			num ^= LocalRecorderStartupError.GetHashCode();
		}
		if (WatchItemError != RecorderWatchItemError.None)
		{
			num ^= WatchItemError.GetHashCode();
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
		if (HasSslErrorWithLocalRecorder)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)8);
			((WriteContext)(ref output)).WriteBool(HasSslErrorWithLocalRecorder);
		}
		if (HasAuthorizationErrorWithLocalRecorder)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)16);
			((WriteContext)(ref output)).WriteBool(HasAuthorizationErrorWithLocalRecorder);
		}
		if (LocalRecorderStartupError)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)24);
			((WriteContext)(ref output)).WriteBool(LocalRecorderStartupError);
		}
		if (WatchItemError != RecorderWatchItemError.None)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)32);
			((WriteContext)(ref output)).WriteEnum((int)WatchItemError);
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
		if (HasSslErrorWithLocalRecorder)
		{
			num += 2;
		}
		if (HasAuthorizationErrorWithLocalRecorder)
		{
			num += 2;
		}
		if (LocalRecorderStartupError)
		{
			num += 2;
		}
		if (WatchItemError != RecorderWatchItemError.None)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)WatchItemError);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void MergeFrom(FailoverRecorderState other)
	{
		if (other != null)
		{
			if (other.HasSslErrorWithLocalRecorder)
			{
				HasSslErrorWithLocalRecorder = other.HasSslErrorWithLocalRecorder;
			}
			if (other.HasAuthorizationErrorWithLocalRecorder)
			{
				HasAuthorizationErrorWithLocalRecorder = other.HasAuthorizationErrorWithLocalRecorder;
			}
			if (other.LocalRecorderStartupError)
			{
				LocalRecorderStartupError = other.LocalRecorderStartupError;
			}
			if (other.WatchItemError != RecorderWatchItemError.None)
			{
				WatchItemError = other.WatchItemError;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
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
		uint num;
		while ((num = ((ParseContext)(ref input)).ReadTag()) != 0 && (num & 7) != 4)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
				break;
			case 8u:
				HasSslErrorWithLocalRecorder = ((ParseContext)(ref input)).ReadBool();
				break;
			case 16u:
				HasAuthorizationErrorWithLocalRecorder = ((ParseContext)(ref input)).ReadBool();
				break;
			case 24u:
				LocalRecorderStartupError = ((ParseContext)(ref input)).ReadBool();
				break;
			case 32u:
				WatchItemError = (RecorderWatchItemError)((ParseContext)(ref input)).ReadEnum();
				break;
			}
		}
	}
}
