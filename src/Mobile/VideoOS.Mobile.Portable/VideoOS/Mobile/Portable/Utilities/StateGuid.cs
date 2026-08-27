using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class StateGuid : StateId<Guid>
{
	public StateGuid(Guid stateId)
		: base(stateId, (Func<Guid>)Guid.NewGuid)
	{
	}

	public StateGuid()
		: this(Guid.NewGuid())
	{
	}
}
