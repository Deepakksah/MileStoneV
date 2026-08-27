using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class SetRecorderShutdownStateCommand : CommonCommandBase
{
	private Guid _recorderId;

	private RecorderShutdownState _state;

	[DataMember]
	public Guid RecorderId
	{
		[DebuggerStepThrough]
		get
		{
			return _recorderId;
		}
		[DebuggerStepThrough]
		set
		{
			_recorderId = value;
		}
	}

	[DataMember]
	public RecorderShutdownState State
	{
		[DebuggerStepThrough]
		get
		{
			return _state;
		}
		[DebuggerStepThrough]
		set
		{
			_state = value;
		}
	}

	public SetRecorderShutdownStateCommand(Guid recorderId, RecorderShutdownState state)
	{
		RecorderId = recorderId;
		_state = state;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
