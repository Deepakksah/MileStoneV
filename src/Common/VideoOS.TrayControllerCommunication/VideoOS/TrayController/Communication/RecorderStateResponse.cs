using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace VideoOS.TrayController.Communication;

[DebuggerDisplay("{ToString(),nq}")]
public sealed class RecorderStateResponse : IMessage<RecorderStateResponse>, IMessage, IEquatable<RecorderStateResponse>, IDeepCloneable<RecorderStateResponse>, IBufferMessage
{
	private static readonly MessageParser<RecorderStateResponse> _parser = new MessageParser<RecorderStateResponse>((Func<RecorderStateResponse>)(() => new RecorderStateResponse()));

	private UnknownFieldSet _unknownFields;

	public const int ServiceStateFieldNumber = 1;

	private ServiceState_Communication serviceState_;

	public const int ConnectStatusFieldNumber = 2;

	private ConnectStatus_Communication connectStatus_;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageParser<RecorderStateResponse> Parser => _parser;

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public static MessageDescriptor Descriptor => TrayControllerReflection.Descriptor.MessageTypes[4];

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
	public RecorderStateResponse()
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public RecorderStateResponse(RecorderStateResponse other)
		: this()
	{
		serviceState_ = other.serviceState_;
		connectStatus_ = other.connectStatus_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public RecorderStateResponse Clone()
	{
		return new RecorderStateResponse(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public override bool Equals(object other)
	{
		return Equals(other as RecorderStateResponse);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public bool Equals(RecorderStateResponse other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("protoc", null)]
	public void MergeFrom(RecorderStateResponse other)
	{
		if (other != null)
		{
			if (other.ServiceState != ServiceState_Communication.Indeterminate)
			{
				ServiceState = other.ServiceState;
			}
			if (other.ConnectStatus != ConnectStatus_Communication.Disconnected)
			{
				ConnectStatus = other.ConnectStatus;
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
				ServiceState = (ServiceState_Communication)((ParseContext)(ref input)).ReadEnum();
				break;
			case 16u:
				ConnectStatus = (ConnectStatus_Communication)((ParseContext)(ref input)).ReadEnum();
				break;
			}
		}
	}
}
