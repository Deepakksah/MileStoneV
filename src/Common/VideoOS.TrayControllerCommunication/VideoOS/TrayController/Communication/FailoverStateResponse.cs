using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace VideoOS.TrayController.Communication;

[DebuggerDisplay("{ToString(),nq}")]
public sealed class FailoverStateResponse : IMessage<FailoverStateResponse>, IMessage, IEquatable<FailoverStateResponse>, IDeepCloneable<FailoverStateResponse>, IBufferMessage
{
	private static readonly MessageParser<FailoverStateResponse> _parser = new MessageParser<FailoverStateResponse>((Func<FailoverStateResponse>)(() => new FailoverStateResponse()));

	private UnknownFieldSet _unknownFields;

	public const int ServiceStateFieldNumber = 1;

	private ServiceState_Communication serviceState_;

	public const int ConnectStatusFieldNumber = 2;

	private ConnectStatus_Communication connectStatus_;

	public const int FailoverRecorderStateFieldNumber = 3;

	private FailoverRecorderState failoverRecorderState_;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageParser<FailoverStateResponse> Parser => _parser;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageDescriptor Descriptor => TrayControllerReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public ServiceState_Communication ServiceState
	{
		get
		{
			return serviceState_;
		}
		set
		{
			serviceState_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public ConnectStatus_Communication ConnectStatus
	{
		get
		{
			return connectStatus_;
		}
		set
		{
			connectStatus_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public FailoverRecorderState FailoverRecorderState
	{
		get
		{
			return failoverRecorderState_;
		}
		set
		{
			failoverRecorderState_ = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public FailoverStateResponse()
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public FailoverStateResponse(FailoverStateResponse other)
		: this()
	{
		serviceState_ = other.serviceState_;
		connectStatus_ = other.connectStatus_;
		failoverRecorderState_ = ((other.failoverRecorderState_ != null) ? other.failoverRecorderState_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public FailoverStateResponse Clone()
	{
		return new FailoverStateResponse(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public override bool Equals(object other)
	{
		return Equals(other as FailoverStateResponse);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool Equals(FailoverStateResponse other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ServiceState != other.ServiceState)
		{
			return false;
		}
		if (ConnectStatus != other.ConnectStatus)
		{
			return false;
		}
		if (!object.Equals(FailoverRecorderState, other.FailoverRecorderState))
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
		if (ServiceState != ServiceState_Communication.Indeterminate)
		{
			num ^= ServiceState.GetHashCode();
		}
		if (ConnectStatus != ConnectStatus_Communication.Disconnected)
		{
			num ^= ConnectStatus.GetHashCode();
		}
		if (failoverRecorderState_ != null)
		{
			num ^= FailoverRecorderState.GetHashCode();
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
		if (ServiceState != ServiceState_Communication.Indeterminate)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)8);
			((WriteContext)(ref output)).WriteEnum((int)ServiceState);
		}
		if (ConnectStatus != ConnectStatus_Communication.Disconnected)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)16);
			((WriteContext)(ref output)).WriteEnum((int)ConnectStatus);
		}
		if (failoverRecorderState_ != null)
		{
			((WriteContext)(ref output)).WriteRawTag((byte)26);
			((WriteContext)(ref output)).WriteMessage((IMessage)(object)FailoverRecorderState);
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
		if (ServiceState != ServiceState_Communication.Indeterminate)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)ServiceState);
		}
		if (ConnectStatus != ConnectStatus_Communication.Disconnected)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)ConnectStatus);
		}
		if (failoverRecorderState_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize((IMessage)(object)FailoverRecorderState);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void MergeFrom(FailoverStateResponse other)
	{
		if (other == null)
		{
			return;
		}
		if (other.ServiceState != ServiceState_Communication.Indeterminate)
		{
			ServiceState = other.ServiceState;
		}
		if (other.ConnectStatus != ConnectStatus_Communication.Disconnected)
		{
			ConnectStatus = other.ConnectStatus;
		}
		if (other.failoverRecorderState_ != null)
		{
			if (failoverRecorderState_ == null)
			{
				FailoverRecorderState = new FailoverRecorderState();
			}
			FailoverRecorderState.MergeFrom(other.FailoverRecorderState);
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
		uint num;
		while ((num = ((ParseContext)(ref input)).ReadTag()) != 0 && (num & 7) != 4)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
				break;
			case 8u:
				ServiceState = (ServiceState_Communication)((ParseContext)(ref input)).ReadEnum();
				break;
			case 16u:
				ConnectStatus = (ConnectStatus_Communication)((ParseContext)(ref input)).ReadEnum();
				break;
			case 26u:
				if (failoverRecorderState_ == null)
				{
					FailoverRecorderState = new FailoverRecorderState();
				}
				((ParseContext)(ref input)).ReadMessage((IMessage)(object)FailoverRecorderState);
				break;
			}
		}
	}
}
