using System;

namespace VideoOS.Platform.EventsAndState;

public abstract class State
{
	public virtual Guid Type { get; }

	public virtual string Source { get; }

	public virtual DateTime Time { get; }

	public virtual Guid? StateGroupId { get; }

	public State(Guid type, string source, Guid? stateGroupId, DateTime time)
	{
		Type = type;
		Source = source;
		StateGroupId = stateGroupId;
		Time = time;
	}

	public override string ToString()
	{
		return FormattableString.Invariant($"{base.ToString()}, Type: {Type}, Source: {Source}{(StateGroupId.HasValue ? FormattableString.Invariant($", StateGroup {StateGroupId}") : string.Empty)}, Time: {Time}");
	}
}
