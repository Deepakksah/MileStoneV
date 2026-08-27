using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class StateId<TId> : IStateId<TId>
{
	private TId _changesStateId;

	private readonly object _changesStateIdLocker = new object();

	private readonly Func<TId> _creator;

	public TId Id
	{
		get
		{
			lock (_changesStateIdLocker)
			{
				return _changesStateId;
			}
		}
	}

	public StateId(TId stateId, Func<TId> creator)
	{
		_changesStateId = stateId;
		_creator = creator;
	}

	public StateId(Func<TId> creator)
		: this(creator(), creator)
	{
	}

	public void OnChange()
	{
		lock (_changesStateIdLocker)
		{
			_changesStateId = _creator();
		}
	}
}
